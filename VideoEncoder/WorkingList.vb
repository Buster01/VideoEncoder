Imports System.ComponentModel

Public Class WorkingList
    Private Sub BgWffmpeg_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BgWffmpeg.DoWork
        Dim ffmpeg_out(6) As String
        Dim ffmpeg_path As String = My.Settings.FFmpegPath
        Dim output_folder As String = ""


        Dim VideoFile As String = DirectCast(e.Argument(0), String)
        Dim hwDecodingParameter As String = DirectCast(e.Argument(1), String)
        Dim AudioProperties As String = DirectCast(e.Argument(2), String)
        Dim VideoProperties As String = DirectCast(e.Argument(3), String)
        Dim SubtitelParameter As String = DirectCast(e.Argument(4), String)

        Dim stderr As String = ""
        Dim stdout As String = ""
        Dim pos As String = 0
        Dim duration As Long = VideoDuration(VideoFile, ffmpeg_path)
        Dim coder_pos As Long = 0
        Dim prozent As Decimal = 0.0
        Dim old_prozent As Decimal = 0.0

        Dim p_ffmpeg As New Process
        Dim ffmpeg_arguments As String = ""
        Dim ProcessProperties As New ProcessStartInfo

        ProcessProperties.FileName = ffmpeg_path & "ffmpeg.exe"
        ProcessProperties.WorkingDirectory = ffmpeg_path
        ProcessProperties.UseShellExecute = False
        ProcessProperties.RedirectStandardOutput = False
        ProcessProperties.RedirectStandardError = True
        ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden
        ProcessProperties.CreateNoWindow = True

        ffmpeg_arguments = hwDecodingParameter & " -y -i " & Chr(34) & VideoFile & Chr(34) & " "
        ffmpeg_arguments = ffmpeg_arguments & AudioProperties

        ffmpeg_arguments = ffmpeg_arguments & " " & SubtitelParameter

        ffmpeg_arguments = ffmpeg_arguments & VideoProperties
        ffmpeg_arguments = ffmpeg_arguments & Chr(34) & output_folder & System.IO.Path.GetFileNameWithoutExtension(VideoFile) & ".mkv" & Chr(34)
        ProcessProperties.Arguments = ffmpeg_arguments

        Dim myProcess As New Process
        myProcess = Process.Start(ProcessProperties)
        Do While Not myProcess.HasExited
            stdout = myProcess.StandardError.ReadLine
            ' stderr = myProcess.StandardOutput.ReadLine
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
                                            BgWffmpeg.ReportProgress(Math.Round(prozent, 1))
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
            If WorkingListInfo(5).Length > 0 Then WorkingListInfo(5) = Strings.Left(WorkingListInfo(5), WorkingListInfo(5).Length - 2).Trim
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
End Class