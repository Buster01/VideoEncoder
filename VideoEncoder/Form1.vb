Imports System.ComponentModel

Public Class Form1
    Dim input_file As String = ""
    Dim input_folder As String = ""
    Dim output_folder As String = ""
    Dim folder As Boolean = False
    Dim ffmpeg_path As String = "C:\ffmpeg\"


    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        folder = CheckBox1.Checked
        If CheckBox1.Checked = True Then
            Label2.Visible = True
            Label2.Text = ""
            lblInputDirectory.Text = ""
            cbFiles.Items.Clear()
        End If
        If CheckBox1.Checked = False Then
            Label2.Visible = False
            Label2.Text = ""
            lblInputDirectory.Text = ""
            cbFiles.Items.Clear()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim file_count As Double = 0
        Dim folder_size As Double = 0
        cbFiles.Items.Clear()

        If folder = True Then
            If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
                input_folder = FolderBrowserDialog1.SelectedPath
                If System.IO.Directory.Exists(input_folder) = True Then
                    lblInputDirectory.Text = input_folder
                    For Each file In New IO.DirectoryInfo(input_folder).GetFiles.OrderBy(Function(s) s.FullName)
                        If file.Extension = ".mkv" Or file.Extension = ".ts" Then
                            cbFiles.Items.Add(file.Name)
                            file_count += 1
                            folder_size = folder_size + file.Length
                        End If
                    Next
                    If folder_size > 1200 And folder_size < 1048000 Then Label2.Text = file_count & " Dateien (" & Math.Round(folder_size / 1024, 2) & " kBytes)"
                    If folder_size > 1048000 And folder_size < 1073741000 Then Label2.Text = file_count & " Dateien (" & Math.Round(folder_size / 1048576, 2) & " MBytes)"
                    If folder_size > 1073741000 Then Label2.Text = file_count & " Dateien (" & Math.Round(folder_size / 1073741824, 2) & " GBytes)"
                End If
            End If
        Else
            OpenFileDialog1.Filter = "Video Dateien|*.mkv;*.ts"
            OpenFileDialog1.Title = "Videodatei auswählen"
            OpenFileDialog1.FileName = ""
            If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                input_file = OpenFileDialog1.FileName
                If System.IO.File.Exists(input_file) = False Then
                    MsgBox("Datei existiert nicht!", vbCritical)
                    OpenFileDialog1.FileName = ""
                    input_file = ""
                Else
                    lblInputDirectory.Text = System.IO.Path.GetDirectoryName(input_file).ToString
                    cbFiles.Items.Add(System.IO.Path.GetFileName(input_file))
                    Dim file_info As New System.IO.FileInfo(input_file)
                    Dim file_size As Double = file_info.Length

                    If file_size > 1200 And file_size < 1048000 Then Label2.Text = Math.Round(file_size / 1024, 2) & " kBytes"
                    If file_size > 1048000 And file_size < 1073741000 Then Label2.Text = Math.Round(file_size / 1048576, 2) & " MBytes"
                    If file_size > 1073741000 Then Label2.Text = Math.Round(file_size / 1073741824, 2) & " GBytes"
                    Label2.Visible = True
                End If
            End If
        End If
        If cbFiles.Items.Count > 0 Then cbFiles.SelectedIndex = 0
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog2.ShowDialog() = DialogResult.OK Then
            output_folder = FolderBrowserDialog2.SelectedPath
            If System.IO.Directory.Exists(output_folder) = True Then
                Label3.Text = output_folder
            End If
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim VideoFiles() As String = DirectCast(e.Argument(0), String())
        Dim AudioProperties As String = DirectCast(e.Argument(1), String)
        Dim VideoProperties As String = DirectCast(e.Argument(2), String)

        Dim stdout As String = ""
        Dim codecposition As Long = 0
        Dim percent As Integer = 0
        Dim old_percent As Integer = 0
        Dim output() As String
        Dim temp_time() As String

        Dim p_ffmpeg As New Process
        Dim ffmpeg_arguments As String = ""
        Dim ProcessProperties As New ProcessStartInfo

        ProcessProperties.FileName = ffmpeg_path & "ffmpeg.exe"
        ProcessProperties.WorkingDirectory = ffmpeg_path
        ProcessProperties.UseShellExecute = False
        ProcessProperties.RedirectStandardOutput = False
        ProcessProperties.RedirectStandardError = True
        ProcessProperties.WindowStyle = ProcessWindowStyle.Normal

        For z = 0 To VideoFiles.Count - 1
            Dim file As String = input_folder & "\" & VideoFiles(z)
            Dim duration As Long = VideoDuration(file, ffmpeg_path)

            ffmpeg_arguments = "-hwaccel dxva2 -y -i " & Chr(34) & file & Chr(34)
            ffmpeg_arguments = ffmpeg_arguments & AudioProperties

            ffmpeg_arguments = ffmpeg_arguments & " -c:s copy"

            ffmpeg_arguments = ffmpeg_arguments & VideoProperties & " -rc vbr_hq -map 0 "
            ffmpeg_arguments = ffmpeg_arguments & Chr(34) & output_folder & System.IO.Path.GetFileNameWithoutExtension(file) & ".mkv" & Chr(34)
            ProcessProperties.Arguments = ffmpeg_arguments

            Dim myProcess As New Process
            myProcess = Process.Start(ProcessProperties)
            Do While Not myProcess.HasExited
                stdout = myProcess.StandardError.ReadLine
                output = Split(stdout, "=")
                If UBound(output) = 7 Then
                    temp_time = Split(Replace(output(5), "bitrate", "").Trim, ":")
                    If temp_time(0) >= 0 Then
                        codecposition = CInt(temp_time(0)) * 3600 ' Stunden
                        codecposition = codecposition + CInt(temp_time(1)) * 60 'Minuten
                        codecposition = codecposition + CInt(Replace(temp_time(2), ".", ","))
                        If codecposition > 0 Then
                            percent = Math.Round((codecposition / duration) * 100, 0)
                            If percent > old_percent Then
                                Me.BackgroundWorker1.ReportProgress(percent)
                                old_percent = percent
                            End If
                        End If
                    End If
                End If
            Loop
            myProcess.WaitForExit()
        Next z
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        cbFiles.Enabled = True
        Button3.Enabled = True
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Me.ProgressBar1.Value = e.ProgressPercentage
        Me.Label4.Text = e.ProgressPercentage & "%"
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ProgressBar1.Maximum = 100
        Me.ProgressBar1.Minimum = 0
        ' Audio Optionen

        With ComboBox5
            .Items.Add("Main")
            .Items.Add("Main10")
            .SelectedIndex = 0
        End With

        With ComboBox6
            .Items.Add("3.1")
            .Items.Add("4.0")
            .Items.Add("4.1")
            .Items.Add("5.0")
            .Items.Add("5.1")
            .Items.Add("5.2")
            .Items.Add("6.0")
            .Items.Add("6.1")
            .Items.Add("6.2")
            .SelectedIndex = 3
        End With

        With cbFiles
            .Items.Clear()
        End With

        With lvFileStreams
            .Columns.Add("ID", 30, HorizontalAlignment.Left)
            .Columns.Add("Type", 60, HorizontalAlignment.Left)
            .Columns.Add("Codec", 100, HorizontalAlignment.Left)
            .Columns.Add("Sprache", 80, HorizontalAlignment.Center)
            .Columns.Add("Frames", 70, HorizontalAlignment.Right)
            .Columns.Add("Standard", 60, HorizontalAlignment.Left)
            .Columns.Add("Encoder", 130, HorizontalAlignment.Left)
            .Columns.Add("Bitrate", 100, HorizontalAlignment.Left)
            .Columns.Add("", 100, HorizontalAlignment.Left)
        End With

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim VideoFiles As New List(Of String)
        Dim AudioParameter As String = ""
        Dim VideoParameter As String = " -c:v "
        Dim z As Integer = 0


        If folder = True Then
            If System.IO.Directory.Exists(input_folder) = True And System.IO.Directory.Exists(output_folder) = True Then
                If Strings.Right(output_folder, 1) <> "\" Then output_folder = output_folder & "\"
                cbFiles.Enabled = False
                Button3.Enabled = False
                BackgroundWorker1.RunWorkerAsync({cbFiles.Items.Cast(Of String).ToArray, AudioParameter, VideoParameter})
            End If
        End If
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFiles.SelectedIndexChanged
        If cbFiles.Items.Count > 0 Then
            lvFileStreams.Items.Clear()
            lvFileStreams.Controls.Clear()
            Dim file As String = lblInputDirectory.Text & "\" & cbFiles.SelectedItem
            Dim streams As Xml.XmlNode = VideoFileStreams(file, ffmpeg_path)

            For z = 0 To streams.ChildNodes.Count - 1
                Dim item As New ListViewItem
                Dim vcCombo As New ComboBox
                AddHandler vcCombo.SelectedIndexChanged, AddressOf vcCombo_SelectedIndexChanged
                Dim brCombo As New ComboBox
                AddHandler brCombo.SelectedIndexChanged, AddressOf brCombo_SelectedIndexChanged

                item = XMLStreamAnalyse(streams.ChildNodes(z))
                lvFileStreams.Items.Add(item)

                'Fülle VideoEncoder
                Select Case item.SubItems(1).Text
                    Case "Video"
                        With vcCombo
                            .Items.Add("copy")
                            .Items.Add("x264")
                            .Items.Add("x265")
                            .Items.Add("Intel QSV H.264")
                            .Items.Add("Intel QSV H.265")
                            .Items.Add("NVidia NVenc H.264")
                            .Items.Add("NVidia NVenc H.265")
                            .SelectedIndex = 0
                        End With

                        With brCombo
                            .Items.Add("------")
                            .SelectedIndex = 0
                        End With

                    Case "Audio"
                        With vcCombo
                            .Items.Add("copy")
                            .Items.Add("AAC")
                            .SelectedIndex = 0
                        End With

                        With brCombo
                            .Items.Add("------")
                            .SelectedIndex = 0
                        End With

                    Case "Untertitel"
                        With vcCombo
                            .Items.Add("copy")
                            .SelectedIndex = 0
                        End With

                        With brCombo
                            .Items.Add("------")
                            .SelectedIndex = 0
                        End With

                End Select

                vcCombo.DropDownStyle = ComboBoxStyle.DropDownList
                brCombo.DropDownStyle = ComboBoxStyle.DropDownList
                vcCombo.Height = item.Bounds.Height
                vcCombo.Width = lvFileStreams.Columns(6).Width
                brCombo.Height = item.Bounds.Height
                brCombo.Width = lvFileStreams.Columns(7).Width
                vcCombo.Location = New Point(lvFileStreams.Items(z).SubItems(5).Bounds.Right, lvFileStreams.Items(z).SubItems(5).Bounds.Y)
                brCombo.Location = New Point(lvFileStreams.Items(z).SubItems(6).Bounds.Right, lvFileStreams.Items(z).SubItems(6).Bounds.Y)
                vcCombo.Parent = lvFileStreams
                brCombo.Parent = lvFileStreams
                vcCombo.Name = "vcCombo" & z.ToString.Trim
                brCombo.Name = "brCombo" & z.ToString.Trim
                lvFileStreams.Controls.Add(vcCombo)
                lvFileStreams.Controls.Add(brCombo)




            Next z
        End If
    End Sub

    Private Sub vcCombo_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim vcCombo As ComboBox = sender
        If vcCombo.Name = "" Then Exit Sub
        Dim brC As String = "br" & Mid(vcCombo.Name, 3)

        Dim si As String = vcCombo.SelectedItem

        For Each ctrl In lvFileStreams.Controls.Find(brC, True)

            Dim brCombo As ComboBox = ctrl
            brCombo.Items.Clear()

            Select Case si
                Case "copy"
                    brCombo.Items.Add("------")
                    brCombo.SelectedIndex = 0

                Case "AAC"
                    brCombo.Items.Add("128 kBit/s")
                    brCombo.Items.Add("192 kBit/s")
                    brCombo.Items.Add("256 kBit/s")
                    brCombo.Items.Add("320 kBit/s")
                    brCombo.Items.Add("384 kBit/s")
                    brCombo.Items.Add("448 kBit/s")
                    brCombo.Items.Add("512 kBit/s")
                    brCombo.SelectedIndex = 1

                Case "x264", "Intel QSV H.264", "NVidia NVenc H.264"
                    brCombo.Items.Add("1000 kBit/s")
                    brCombo.Items.Add("2000 kBit/s")
                    brCombo.Items.Add("2500 kBit/s")
                    brCombo.Items.Add("3000 kBit/s")
                    brCombo.Items.Add("3500 kBit/s")
                    brCombo.Items.Add("4000 kBit/s")
                    brCombo.Items.Add("4500 kBit/s")
                    brCombo.Items.Add("5000 kBit/s")
                    brCombo.Items.Add("5500 kBit/s")
                    brCombo.Items.Add("6000 kBit/s")
                    brCombo.Items.Add("6500 kBit/s")
                    brCombo.Items.Add("7000 kBit/s")
                    brCombo.Items.Add("7500 kBit/s")
                    brCombo.Items.Add("8000 kBit/s")
                    brCombo.Items.Add("8500 kBit/s")
                    brCombo.Items.Add("9000 kBit/s")
                    brCombo.Items.Add("9500 kBit/s")
                    brCombo.SelectedIndex = 8

                Case Else
                    brCombo.Items.Add("1000 kBit/s")
                    brCombo.Items.Add("2000 kBit/s")
                    brCombo.Items.Add("2500 kBit/s")
                    brCombo.Items.Add("3000 kBit/s")
                    brCombo.Items.Add("3500 kBit/s")
                    brCombo.Items.Add("4000 kBit/s")
                    brCombo.Items.Add("4500 kBit/s")
                    brCombo.Items.Add("5000 kBit/s")
                    brCombo.Items.Add("5500 kBit/s")
                    brCombo.Items.Add("6000 kBit/s")
                    brCombo.Items.Add("6500 kBit/s")
                    brCombo.Items.Add("7000 kBit/s")
                    brCombo.Items.Add("7500 kBit/s")
                    brCombo.Items.Add("8000 kBit/s")
                    brCombo.Items.Add("8500 kBit/s")
                    brCombo.Items.Add("9000 kBit/s")
                    brCombo.Items.Add("9500 kBit/s")
                    brCombo.SelectedIndex = 4

            End Select
            Exit For
        Next




    End Sub

    Private Sub brCombo_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub


End Class

