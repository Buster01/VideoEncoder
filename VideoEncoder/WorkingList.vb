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
            .Columns.Add("Datei", 60, HorizontalAlignment.Left)
            .Columns.Add("test", 120, HorizontalAlignment.Left)
            .Columns.Add("Sprache", 80, HorizontalAlignment.Center)
            .Columns.Add("Frames", 70, HorizontalAlignment.Right)
            .Columns.Add("Standard", 100, HorizontalAlignment.Left)
            .Columns.Add("Encoder", 130, HorizontalAlignment.Left)
            .Columns.Add("Bitrate", 100, HorizontalAlignment.Left)
        End With

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














    Public Function UpdateWorkingList()
        lvWorkingList.Items.Clear()
        lvWorkingList.Controls.Clear()
        Dim t As String = ""

        Dim order As Xml.XmlNode = Main.CodecQueue.SelectSingleNode("WorkingQueue")

        For Each CodingOrder As Xml.XmlNode In order.ChildNodes
            Dim item As New ListViewItem(CodingOrder.Attributes("id").Value)
            lvWorkingList.Items.Add(item)



        Next



    End Function
End Class