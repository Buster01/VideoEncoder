Public Class Settings
    Private Sub Einstellungen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = My.Settings.FFmpegPath
        cbFFmpegLog.Checked = My.Settings.FFmpegLog
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If System.IO.Directory.Exists(Label1.Text) = True Then
            FolderBrowserDialog1.SelectedPath = Label1.Text
        End If

        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            If System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath) = True Then
                Label1.Text = FolderBrowserDialog1.SelectedPath
            End If
        End If

        If System.IO.File.Exists(FolderBrowserDialog1.SelectedPath & "\ffmpeg.exe") = False Then
            MsgBox("FFmpeg wurde nicht im Pfad gefunden!", vbCritical, "FFmpeg")
            Exit Sub
        End If
        If System.IO.File.Exists(FolderBrowserDialog1.SelectedPath & "\ffprobe.exe") = False Then
            MsgBox("FFprobe wurde nicht im Pfad gefunden!", vbCritical, "FFprobe")
            Exit Sub
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.FFmpegPath = Label1.Text.ToString
        My.Settings.FFmpegLog = cbFFmpegLog.Checked
        My.Settings.Save()
        Me.Close()
    End Sub
End Class