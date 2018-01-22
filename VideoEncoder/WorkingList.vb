Imports System.ComponentModel

Public Class WorkingList
    Private Sub BgWffmpeg_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BgWffmpeg.DoWork
        Dim Order As New Xml.XmlDocument
        Order.LoadXml(e.Argument)

        Dim OrderNode As Xml.XmlNode = Order.SelectSingleNode("Order")
        If IsNothing(OrderNode) = True Then Exit Sub

        Dim ffmpeg_path As String = My.Settings.FFmpegPath
        If Strings.Right(ffmpeg_path, 1) <> "\" Then ffmpeg_path = ffmpeg_path & "\"
        Dim HWEncoding As String = ""
        Dim DeInterlace As String = ""
        Dim DTSFix As String = ""
        Dim CodecProfile As String = ""
        Dim CodecLevel As String = ""
        Dim FFVideoParameter As String = ""
        Dim FFAdudioParameter As String = ""
        Dim FFSubtitleParameter As String = ""
        Dim AudioStreamID As Integer = 0
        Dim SubtitleStreamID As Integer = 0

        Dim stderr As String = ""
        Dim stdout As String = ""
        Dim pos As String = 0
        Dim duration As Long = 0
        Dim coder_pos As Long = 0
        Dim prozent As Decimal = 0.0
        Dim old_prozent As Decimal = 0.0

        Dim p_ffmpeg As New Process
        Dim ffmpeg_arguments As String = ""
        Dim ProcessProperties As New ProcessStartInfo

        'InputFile
        Dim VideoFile As String = OrderNode.Attributes("InputFolder").Value
        If Strings.Right(VideoFile, 1) = "\" Then
            VideoFile = VideoFile & OrderNode.Attributes("InputFile").Value
        Else
            VideoFile = VideoFile & "\" & OrderNode.Attributes("InputFile").Value
        End If
        duration = VideoDuration(VideoFile, ffmpeg_path)

        'OutputFile
        Dim OutputFile As String = OrderNode.Attributes("OutputPath").Value
        If Strings.Right(OutputFile, 1) = "\" Then
            OutputFile = OutputFile & System.IO.Path.GetFileNameWithoutExtension(VideoFile) & ".mkv"
        Else
            OutputFile = OutputFile & "\" & System.IO.Path.GetFileNameWithoutExtension(VideoFile) & ".mkv"
        End If

        If OrderNode.Attributes("CodecHQEncoding").Value = True Then HWEncoding = "-hwaccel dxva2 " Else HWEncoding = ""
        If OrderNode.Attributes("CodecDeinterlace").Value = "yadif" Then DeInterlace = "-vf yadif=1 " Else DeInterlace = ""
        If OrderNode.Attributes("CodecDTSFix").Value = True Then DTSFix = "-max_muxing_queue_size 3000 " Else DTSFix = ""
        If OrderNode.Attributes("CodecProfil").Value = "------" Then CodecProfile = "" Else CodecProfile = "-profile:v " & OrderNode.Attributes("CodecProfil").Value.ToLower & " "
        If OrderNode.Attributes("CodecLevel").Value = "------" Then CodecLevel = "" Else CodecLevel = "-level " & OrderNode.Attributes("CodecLevel").Value & " "

        For Each streams As Xml.XmlNode In OrderNode.ChildNodes
            Select Case streams.Attributes("StreamType").Value
                Case "Video"
                    Dim VCodec As String = streams.Attributes("StreamCodec").Value.ToString.ToLower
                    If VCodec = "copy" Then
                        FFVideoParameter = "-c:v copy "
                    Else
                        Select Case VCodec
                            Case "x264"
                                FFVideoParameter = "-c:v libx264 "

                            Case "x265"
                                FFVideoParameter = "-c:v libx265 "

                            Case "intel qsv H.264"
                                FFVideoParameter = "-c:v h264_qsv "

                            Case "intel qsv H.265"
                                FFVideoParameter = "-c:v hevc_qsv "

                            Case "nvidia nvenc h.264"
                                FFVideoParameter = "-c:v h264_nvenc "

                            Case "nvidia nvenc h.265"
                                FFVideoParameter = "-c:v hevc_nvenc "
                        End Select
                    End If
                    FFVideoParameter = FFVideoParameter & "-b:v " & Strings.Mid(streams.Attributes("StreamBitrate").Value, 1, 4).Trim & "k "


                Case "Audio"
                    Dim ACodec As String = streams.Attributes("StreamCodec").Value
                    If ACodec = "copy" Then
                        FFAdudioParameter = FFAdudioParameter & "-c:a:" & AudioStreamID.ToString.Trim & " copy "
                    Else
                        FFAdudioParameter = FFAdudioParameter & "-c:a:" & AudioStreamID.ToString.Trim & " " & ACodec.ToLower & " -b:a:" & AudioStreamID.ToString.Trim & " " & Strings.Mid(streams.Attributes("StreamBitrate").Value, 1, 3).ToString & "k "
                    End If
                    If streams.Attributes("StreamDefault").Value Then
                        FFAdudioParameter = FFAdudioParameter & "-disposition:a:" & AudioStreamID.ToString.Trim & " default "
                    Else
                        FFAdudioParameter = FFAdudioParameter & "-disposition:a:" & AudioStreamID.ToString.Trim & " none "
                    End If
                    AudioStreamID += 1


                Case "Untertitel"
                    FFSubtitleParameter = FFSubtitleParameter & "-c:s:" & SubtitleStreamID.ToString.Trim & " copy "
                    If streams.Attributes("StreamDefault").Value Then
                        FFSubtitleParameter = FFSubtitleParameter & "-disposition:s:" & SubtitleStreamID.ToString.Trim & " default "
                    Else
                        FFSubtitleParameter = FFSubtitleParameter & "-disposition:s:" & SubtitleStreamID.ToString.Trim & " none "
                    End If
                    SubtitleStreamID += 1

            End Select
        Next
        AudioStreamID = 0
        SubtitleStreamID = 0

        'FFMpeg Prozess
        ProcessProperties.FileName = ffmpeg_path & "ffmpeg.exe"
        ProcessProperties.WorkingDirectory = ffmpeg_path
        ProcessProperties.UseShellExecute = False
        ProcessProperties.RedirectStandardOutput = False
        ProcessProperties.RedirectStandardError = True
        ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden
        ProcessProperties.CreateNoWindow = True

        ffmpeg_arguments = HWEncoding & "-y -i " & Chr(34) & VideoFile & Chr(34) & " "
        ffmpeg_arguments = ffmpeg_arguments & FFAdudioParameter

        ffmpeg_arguments = ffmpeg_arguments & FFSubtitleParameter

        ffmpeg_arguments = ffmpeg_arguments & FFVideoParameter
        ffmpeg_arguments = ffmpeg_arguments & Chr(34) & OutputFile & Chr(34)
        ProcessProperties.Arguments = ffmpeg_arguments

        Dim ffmpeg_out(6) As String
        Dim myProcess As New Process
        myProcess = Process.Start(ProcessProperties)
        Do While Not myProcess.HasExited
            stdout = myProcess.StandardError.ReadLine
            'stderr = myProcess.StandardOutput.ReadLine
            If Strings.Left(stdout, 5).ToString.ToLower = "frame" Then
                pos = InStr(stdout, "fps=")
                If pos > 0 Then
                    'frames
                    ffmpeg_out(0) = Mid(stdout, 7, pos - 7).Trim
                    stdout = Mid(stdout, pos)
                    pos = InStr(stdout, "q=")
                    If pos > 0 Then
                        'fps
                        ffmpeg_out(1) = Mid(stdout, 5, pos - 5).Trim
                        stdout = Mid(stdout, pos)
                        pos = InStr(stdout, "size=")
                        If pos > 0 Then
                            'Quality
                            ffmpeg_out(2) = Mid(stdout, 6, pos - 6).Trim
                            stdout = Mid(stdout, pos)
                            pos = InStr(stdout, "time=")
                            If pos > 0 Then
                                'size
                                ffmpeg_out(3) = Mid(stdout, 6, pos - 6).Trim
                                Replace(ffmpeg_out(3), "kb", "")
                                stdout = Mid(stdout, pos)
                                pos = InStr(stdout, "bitrate=")
                                If pos > 0 Then
                                    'Time
                                    ffmpeg_out(4) = Mid(stdout, 6, pos - 6).Trim
                                    stdout = Mid(stdout, pos)
                                    pos = InStr(stdout, "speed=")
                                    If pos > 0 Then
                                        'Bitrate
                                        ffmpeg_out(5) = Mid(stdout, 9, pos - 9).Trim
                                        ffmpeg_out(5) = Replace(ffmpeg_out(5), "kbits/s", "")
                                        stdout = Mid(stdout, pos)
                                        'Speed
                                        ffmpeg_out(6) = Mid(stdout, 7).Trim
                                        ffmpeg_out(6) = Replace(ffmpeg_out(6), "x", "")
                                        coder_pos = (CDec(Mid(ffmpeg_out(4), 1, 2)) * 3600) + (CDec(Mid(ffmpeg_out(4), 4, 2) * 60)) + CDec(Replace(Strings.Mid(ffmpeg_out(4), 7), ".", ","))
                                        prozent = (coder_pos / duration) * 100
                                        If Math.Round(prozent, 1) > Math.Round(old_prozent, 1) Then
                                            BgWffmpeg.ReportProgress(Nothing, {ffmpeg_out})
                                            old_prozent = prozent
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Loop
        stdout = stdout & myProcess.StandardError.ReadLine
        stderr = myProcess.StandardOutput.ReadLine
        myProcess.WaitForExit()
        BgWffmpeg.CancelAsync()
    End Sub

    Private Sub WorkingList_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.WorkingListPosition = Me.Location
        My.Settings.WorkingListSize = Me.Size
    End Sub

    Private Sub WorkingList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.WorkingListSize.Height > 100 And My.Settings.WorkingListSize.Width > 100 Then
            Me.Size = My.Settings.WorkingListSize
        End If
        GroupBox1.Height = Me.Size.Height - 75
        GroupBox1.Width = Me.Size.Width - 30
        lvWorkingList.Height = GroupBox1.Size.Height - 30
        lvWorkingList.Width = GroupBox1.Size.Width - 15

        With lvWorkingList
            .Columns.Add("ID", 30, HorizontalAlignment.Left)
            .Columns.Add("Quelle", 240, HorizontalAlignment.Left)
            .Columns.Add("Ziel", 120, HorizontalAlignment.Left)
            .Columns.Add("Video", 150, HorizontalAlignment.Left)
            .Columns.Add("Audio", 150, HorizontalAlignment.Left)
            .Columns.Add("Untertitel", 120, HorizontalAlignment.Left)
            .Columns.Add("HW Decoder", 90, HorizontalAlignment.Center)
            .Columns.Add("Deinterlace", 80, HorizontalAlignment.Center)
            .Columns.Add("Status", 80, HorizontalAlignment.Center)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Regular)
        End With

        Call UpdateWorkingList()
    End Sub

    Private Sub WorkingList_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.WorkingListSize = Me.Size
        GroupBox1.Height = Me.Size.Height - 75
        GroupBox1.Width = Me.Size.Width - 30
        lvWorkingList.Height = GroupBox1.Size.Height - 30
        lvWorkingList.Width = GroupBox1.Size.Width - 15
    End Sub

    Private Sub WorkingList_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        GroupBox1.Height = Me.Size.Height - 75
        GroupBox1.Width = Me.Size.Width - 30
        lvWorkingList.Height = GroupBox1.Size.Height - 30
        lvWorkingList.Width = GroupBox1.Size.Width - 15
    End Sub

    Public Sub UpdateWorkingList()
        lvWorkingList.Items.Clear()
        lvWorkingList.Controls.Clear()

        Dim order As Xml.XmlNode = Main.CodecQueue.SelectSingleNode("WorkingQueue")
        Dim z As Long = 0

        For Each CodingOrder As Xml.XmlNode In order.ChildNodes
            Dim WorkingListInfo(8) As String
            Dim item As New ListViewItem
            Dim btnStopDelete As New Button
            AddHandler btnStopDelete.Click, AddressOf btnStopDelete_SelectedIndexChanged

            WorkingListInfo(0) = CodingOrder.Attributes("id").Value
            WorkingListInfo(1) = CodingOrder.Attributes("InputFile").Value
            WorkingListInfo(2) = CodingOrder.Attributes("OutputPath").Value

            For Each stream As Xml.XmlNode In CodingOrder.ChildNodes
                ' Video Stream
                If stream.Attributes("StreamType").Value = "Video" Then
                    WorkingListInfo(3) = stream.Attributes("StreamCodec").Value
                    If stream.Attributes("StreamCodec").Value <> "copy" Then
                        WorkingListInfo(3) = WorkingListInfo(3) & " (" & stream.Attributes("StreamBitrate").Value & ")"
                    End If
                    WorkingListInfo(3) = Replace(WorkingListInfo(3), "NVidia", "").Trim
                End If

                'Audio Stream
                If stream.Attributes("StreamType").Value = "Audio" Then
                    If stream.Attributes("StreamCodec").Value <> "copy" Then
                        WorkingListInfo(4) = WorkingListInfo(4) & stream.Attributes("StreamID").Value & "." & stream.Attributes("StreamCodec").Value & " (" & stream.Attributes("StreamBitrate").Value & ") | "
                    Else
                        WorkingListInfo(4) = WorkingListInfo(4) & stream.Attributes("StreamID").Value & "." & stream.Attributes("StreamCodec").Value & " | "
                    End If
                End If
                'Untertitel
                If stream.Attributes("StreamType").Value = "Untertitel" Then
                    If stream.Attributes("StreamCodec").Value = "copy" And stream.Attributes("StreamDefault").Value = "True" Then
                        WorkingListInfo(5) = WorkingListInfo(5) & "copy(X) | "
                    End If
                    If stream.Attributes("StreamCodec").Value = "copy" And stream.Attributes("StreamDefault").Value = "False" Then
                        WorkingListInfo(5) = WorkingListInfo(5) & "copy | "
                    End If

                End If
            Next
            'HW Decoding
            If CodingOrder.Attributes("HWdecoding").Value = "True" Then
                WorkingListInfo(6) = "DirectX VA"
            Else
                WorkingListInfo(6) = "Software"
            End If
            'DeInterlace
            WorkingListInfo(7) = CodingOrder.Attributes("CodecDeinterlace").Value
            WorkingListInfo(8) = " "

            If WorkingListInfo(4).Length > 0 Then WorkingListInfo(4) = Strings.Left(WorkingListInfo(4), WorkingListInfo(4).Length - 2).Trim
            If IsNothing(WorkingListInfo(5)) = False Then If WorkingListInfo(5).Length > 0 Then WorkingListInfo(5) = Strings.Left(WorkingListInfo(5), WorkingListInfo(5).Length - 2).Trim
            item = New ListViewItem(WorkingListInfo)

            If CodingOrder.Attributes("State").Value <> "delete" Then
                lvWorkingList.Items.Add(item)
                btnStopDelete.Text = "X"
                btnStopDelete.TextAlign = ContentAlignment.TopCenter
                btnStopDelete.Height = item.Bounds.Height
                btnStopDelete.Width = item.SubItems(8).Bounds.Width
                btnStopDelete.Name = "btnStopDelete" & CodingOrder.Attributes("id").Value.ToString.Trim
                btnStopDelete.Location = New Point(item.SubItems(7).Bounds.Right, item.SubItems(7).Bounds.Y)
                btnStopDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 7, System.Drawing.FontStyle.Regular)
                lvWorkingList.Controls.Add(btnStopDelete)
                z += 1
            End If
        Next
    End Sub

    Private Sub lvWorkingList_ColumnWidthChanging(sender As Object, e As ColumnWidthChangingEventArgs) Handles lvWorkingList.ColumnWidthChanging
        Dim btnStopDelete As New Button
        Dim z As Long = 0

        For Each lvWorkingListItem As ListViewItem In lvWorkingList.Items
            For Each lvi_ctrl In lvWorkingList.Controls.Find("btnStopDelete" & z.ToString, True)
                btnStopDelete = lvi_ctrl
                btnStopDelete.Width = lvWorkingListItem.SubItems(8).Bounds.Width
                btnStopDelete.Location = New Point(lvWorkingListItem.SubItems(7).Bounds.Right, lvWorkingListItem.SubItems(7).Bounds.Y)
                Exit For
            Next
            z += 1
        Next
    End Sub

    Public Sub btnStopDelete_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim btnStopDelete As Button = sender
        If btnStopDelete.Name = "" Then Exit Sub

        Dim row As ListViewItem
        Dim OrderID As Long = CLng(Replace(btnStopDelete.Name, "btnStopDelete", ""))
        Dim order As Xml.XmlNode = Main.CodecQueue.SelectSingleNode("WorkingQueue")

        'Eintrag löschen
        For Each row In lvWorkingList.Items
            If CLng(row.Text.ToString) = OrderID Then
                lvWorkingList.Controls.Remove(btnStopDelete)
                row.Remove()
                Exit For
            End If
        Next

        'Button an richtig stelle
        For Each lvWorkingListItem As ListViewItem In lvWorkingList.Items
            For Each lvi_ctrl In lvWorkingList.Controls.Find("btnStopDelete" & lvWorkingListItem.SubItems(0).Text.ToString.Trim, True)
                btnStopDelete = lvi_ctrl
                btnStopDelete.Width = lvWorkingListItem.SubItems(8).Bounds.Width
                btnStopDelete.Location = New Point(lvWorkingListItem.SubItems(7).Bounds.Right, lvWorkingListItem.SubItems(7).Bounds.Y)
                Exit For
            Next
        Next

        'Encoding Auftrag löschen
        For Each CodingOrder As Xml.XmlNode In order.ChildNodes
            If CodingOrder.Attributes("id").Value = OrderID Then
                order.RemoveChild(CodingOrder)
                Exit For
            End If
        Next

    End Sub

    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        Dim ffmpeg_path As String = My.Settings.FFmpegPath

        If BgWffmpeg.IsBusy = True Then
            StartToolStripMenuItem.Enabled = False
        Else
            'Prüfen ob FFmpeg vorhanden ist
            If System.IO.File.Exists(ffmpeg_path & "\ffmpeg.exe") = False Then
                MsgBox("FFMPEG ist nicht zu finden")
                Exit Sub
            End If
            If System.IO.File.Exists(ffmpeg_path & "\ffprobe.exe") = False Then
                MsgBox("FFProbe ist nicht zu finden")
                Exit Sub
            End If
            Timer1.Enabled = True

        End If
    End Sub

    Private Sub PauseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PauseToolStripMenuItem.Click
        Timer1.Enabled = False
        PauseToolStripMenuItem.Enabled = False
    End Sub

    Private Sub BgWffmpeg_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgWffmpeg.RunWorkerCompleted
        StartToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Interval = 2000
        Dim ffmpeg_path As String = My.Settings.FFmpegPath

        'TransportXML für BGW erstellen
        Dim OrderDoc As New Xml.XmlDocument

        If BgWffmpeg.IsBusy = True Then Exit Sub
        'Prüfen ob FFmpeg vorhanden ist
        If System.IO.File.Exists(ffmpeg_path & "\ffmpeg.exe") = False Then
            MsgBox("FFMPEG ist nicht zu finden")
            Exit Sub
        End If
        If System.IO.File.Exists(ffmpeg_path & "\ffprobe.exe") = False Then
            MsgBox("FFProbe ist nicht zu finden")
            Exit Sub
        End If

        Dim order As Xml.XmlNode = Main.CodecQueue.SelectSingleNode("WorkingQueue")
        Dim z As Long = 0


        For Each CodingOrder As Xml.XmlNode In order.ChildNodes
            If CodingOrder.Attributes("State").Value = "waiting" Then
                'BGW Starten und mit parameter versorgen
                StartToolStripMenuItem.Enabled = False
                BgWffmpeg.WorkerSupportsCancellation = True
                BgWffmpeg.WorkerReportsProgress = True
                CodingOrder.Attributes("State").Value = "in progress"

                Dim tmpNode As Xml.XmlNode
                tmpNode = OrderDoc.CreateElement("Order")
                tmpNode = OrderDoc.ImportNode(CodingOrder, True)

                OrderDoc.AppendChild(tmpNode)
                BgWffmpeg.RunWorkerAsync(OrderDoc.OuterXml.ToString)
                Exit For
            End If
        Next
    End Sub

    Private Sub BgWffmpeg_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BgWffmpeg.ProgressChanged
        Dim t() As String

        t = DirectCast(e.UserState(0), String())
    End Sub
End Class