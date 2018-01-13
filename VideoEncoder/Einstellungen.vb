﻿Public Class Einstellungen
    Private Sub Einstellungen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = My.Settings.FFmpegPath
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
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.Item("FFmpegPath") = Label1.Text.ToString
        My.Settings.Save()
        Me.Close()
    End Sub
End Class