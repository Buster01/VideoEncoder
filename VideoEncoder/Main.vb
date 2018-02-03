Imports System.ComponentModel

Public Class Main
    Dim input_file As String = ""
    Dim input_folder As String = ""
    Dim output_folder As String = ""
    Dim folder As Boolean = False
    Dim ffmpeg_path As String = My.Settings.FFmpegPath
    Dim ffmpeg_out(7) As String
    Public CodecQueue As New Xml.XmlDocument



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
        ffmpeg_path = My.Settings.FFmpegPath
        If ffmpeg_path.Length = 0 Then
            MsgBox("Bitte definieren Sie den Pfad zu FFmpeg!", vbCritical, "Fehler!")
            Settings.Show()
            Exit Sub
        End If
        If Strings.Right(ffmpeg_path, 1) <> "\" Then ffmpeg_path = ffmpeg_path & "\"

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

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        cbFiles.Enabled = True
        Button3.Enabled = True
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Encoding Queue erzeugen
        Dim FirstNode As Xml.XmlNode
        CodecQueue.CreateXmlDeclaration("1.0", "UTF-8", "")
        FirstNode = CodecQueue.CreateElement("WorkingQueue")
        CodecQueue.AppendChild(FirstNode)

        ' Audio Optionen
        With ComboBox5
            .Items.Add("------")
            .SelectedIndex = 0
            .Enabled = False
        End With

        With ComboBox6
            .Items.Add("------")
            .SelectedIndex = 0
            .Enabled = False
        End With

        With cbFiles
            .Items.Clear()
        End With

        With lvFileStreams
            .Columns.Add("ID", 30, HorizontalAlignment.Left)
            .Columns.Add("Type", 60, HorizontalAlignment.Left)
            .Columns.Add("Codec", 120, HorizontalAlignment.Left)
            .Columns.Add("Sprache", 80, HorizontalAlignment.Center)
            .Columns.Add("Frames", 70, HorizontalAlignment.Right)
            .Columns.Add("Standard", 100, HorizontalAlignment.Left)
            .Columns.Add("Encoder", 130, HorizontalAlignment.Left)
            .Columns.Add("Bitrate", 100, HorizontalAlignment.Left)
        End With

        With cbDeInterlace
            .Items.Add("------")
            .Items.Add("yadif")
            .SelectedIndex = 0
        End With

        If ffmpeg_path = "" Then
            MsgBox("Bitte wählen Sie ein Pfad zum FFmpeg!", vbCritical, "FFmpeg Pfad")
            Settings.Show()
        Else
            ffmpeg_path = ffmpeg_path & "\"
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim qid As Long = 0
        Dim chNodes As Long = 0
        Dim z As Long = 0
        Dim CodecQueueChild As Xml.XmlNode = CodecQueue.SelectSingleNode("WorkingQueue")

        If input_folder = "" Then
            MsgBox("Bitte ein Quellpfad wählen!", vbInformation, "keine Quellpfad!")
            Exit Sub
        End If
        If output_folder = "" Then
            MsgBox("Bitte ein Zielpfad wählen!", vbInformation, "keine Zielpfad!")
            Exit Sub
        End If
        If cbFiles.SelectedItem = "" Then
            MsgBox("Quellpfad enhält keine Videodateien!", vbInformation, "keine Videodateien!")
            Exit Sub
        End If
        If ffmpeg_path.Length = 0 Then
            MsgBox("Bitte wählen Sie ein Pfad zum FFmpeg!", vbCritical, "FFmpeg Pfad")
            Settings.Show()
            Exit Sub
        End If

        ' letzte id in Encodingliste ermittlen
        If CodecQueueChild.ChildNodes.Count = 0 Then
            qid = 0
        Else
            For chNodes = 0 To CodecQueueChild.ChildNodes.Count - 1
                If CodecQueueChild.ChildNodes(chNodes).Name = "Order" Then
                    If CodecQueueChild.ChildNodes(chNodes).Attributes("id").Value > qid Then qid = CodecQueueChild.ChildNodes(chNodes).Attributes("id").Value
                End If
            Next
            qid += 1
        End If

        ' neuer Eintrag in Endcodingliste hinzufügen
        'Order id generieren
        Dim EncOrder As Xml.XmlNode = CodecQueue.CreateElement("Order")
        Dim EncOrderAttrID As Xml.XmlAttribute = CodecQueue.CreateAttribute("id")
        EncOrderAttrID.Value = qid

        'Order Status
        Dim EncOrderAttrState As Xml.XmlAttribute = CodecQueue.CreateAttribute("State")
        EncOrderAttrState.Value = "waiting"
        EncOrder.Attributes.Append(EncOrderAttrState)

        'Order Parameter HW Decoding
        Dim EncoderStreamAttrHWdecoding As Xml.XmlAttribute = CodecQueue.CreateAttribute("HWdecoding")
        EncoderStreamAttrHWdecoding.Value = CheckBox3.Checked
        EncOrder.Attributes.Append(EncOrderAttrID)
        EncOrder.Attributes.Append(EncoderStreamAttrHWdecoding)

        'Order InputFile
        Dim EncoderInputFolder As Xml.XmlAttribute = CodecQueue.CreateAttribute("InputFolder")
        EncoderInputFolder.Value = input_folder
        EncOrder.Attributes.Append(EncoderInputFolder)

        'Order InputFile
        Dim EncoderInputFile As Xml.XmlAttribute = CodecQueue.CreateAttribute("InputFile")
        EncoderInputFile.Value = cbFiles.SelectedItem
        EncOrder.Attributes.Append(EncoderInputFile)

        'Order Output Path
        Dim EncoderOutputPath As Xml.XmlAttribute = CodecQueue.CreateAttribute("OutputPath")
        EncoderOutputPath.Value = output_folder
        EncOrder.Attributes.Append(EncoderOutputPath)

        'Codec Profil
        Dim EncoderVideoProfil As Xml.XmlAttribute = CodecQueue.CreateAttribute("CodecProfil")
        EncoderVideoProfil.Value = ComboBox5.SelectedItem
        EncOrder.Attributes.Append(EncoderVideoProfil)

        'Codec level
        Dim EncoderVideoLevel As Xml.XmlAttribute = CodecQueue.CreateAttribute("CodecLevel")
        EncoderVideoLevel.Value = ComboBox6.SelectedItem
        EncOrder.Attributes.Append(EncoderVideoLevel)

        ' VBR HQ Encoding
        Dim EncoderVBRhqEncoding As Xml.XmlAttribute = CodecQueue.CreateAttribute("CodecHQEncoding")
        EncoderVBRhqEncoding.Value = CheckBox2.Checked
        EncOrder.Attributes.Append(EncoderVBRhqEncoding)

        'Codec Filter DeInterlace
        Dim EncoderDeinterlace As Xml.XmlAttribute = CodecQueue.CreateAttribute("CodecDeinterlace")
        EncoderDeinterlace.Value = cbDeInterlace.SelectedItem.ToString
        EncOrder.Attributes.Append(EncoderDeinterlace)

        'DTS Fix
        Dim EncoderDTSfix As Xml.XmlAttribute = CodecQueue.CreateAttribute("CodecDTSFix")
        EncoderDTSfix.Value = CheckBox4.Checked
        EncOrder.Attributes.Append(EncoderDTSfix)

        'Listview mit Streams auslesen
        For Each stream_item As ListViewItem In lvFileStreams.Items
            Dim EncOrderStream As Xml.XmlNode = CodecQueue.CreateElement("Stream")
            ' Stream ID
            Dim EncoderStreamAttrID As Xml.XmlAttribute = CodecQueue.CreateAttribute("StreamID")
            EncoderStreamAttrID.Value = stream_item.SubItems(0).Text
            EncOrderStream.Attributes.Append(EncoderStreamAttrID)

            'Stream Type
            Dim EncoderStreamAttrType As Xml.XmlAttribute = CodecQueue.CreateAttribute("StreamType")
            EncoderStreamAttrType.Value = stream_item.SubItems(1).Text
            EncOrderStream.Attributes.Append(EncoderStreamAttrType)

            'Codec, Bitrate,default
            Dim vcCombo As New ComboBox
            Dim EncoderStreamAttrCodec As Xml.XmlAttribute = CodecQueue.CreateAttribute("StreamCodec")
            Dim brCombo As New ComboBox
            Dim EncoderStreamAttrBitrate As Xml.XmlAttribute = CodecQueue.CreateAttribute("StreamBitrate")
            Dim defckb As New CheckBox
            Dim EncoderStramAttrDefault As Xml.XmlAttribute = CodecQueue.CreateAttribute("StreamDefault")

            ' Finde Combobox mit Codec Auswahl
            For Each vc_ctrl In lvFileStreams.Controls.Find("vcCombo" & z.ToString, True)
                vcCombo = vc_ctrl
            Next
            EncoderStreamAttrCodec.Value = vcCombo.SelectedItem
            EncOrderStream.Attributes.Append(EncoderStreamAttrCodec)

            'Finde Combobx mit Bitrate
            For Each br_ctrl In lvFileStreams.Controls.Find("brCombo" & z.ToString, True)
                brCombo = br_ctrl
            Next
            EncoderStreamAttrBitrate.Value = brCombo.SelectedItem
            EncOrderStream.Attributes.Append(EncoderStreamAttrBitrate)

            ' Finde "Standard - Checkbox
            For Each def_ckbox In lvFileStreams.Controls.Find("defCheck" & z.ToString, True)
                defckb = def_ckbox
            Next
            EncoderStramAttrDefault.Value = defckb.Checked
            EncOrderStream.Attributes.Append(EncoderStramAttrDefault)
            EncOrder.AppendChild(EncOrderStream)
            z += 1
        Next

        CodecQueue.DocumentElement.AppendChild(EncOrder)
        If WorkingList.Visible = True Then WorkingList.UpdateWorkingList()

    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbFiles.SelectedIndexChanged
        If cbFiles.Items.Count > 0 Then
            lvFileStreams.Items.Clear()
            lvFileStreams.Controls.Clear()
            Dim file As String = lblInputDirectory.Text & "\" & cbFiles.SelectedItem
            Dim streams As Xml.XmlNode = VideoFileStreams(file, ffmpeg_path)
            If IsNothing(streams) = True Then Exit Sub

            For z = 0 To streams.ChildNodes.Count - 1
                Dim item As New ListViewItem
                Dim vcCombo As New ComboBox
                AddHandler vcCombo.SelectedIndexChanged, AddressOf vcCombo_SelectedIndexChanged
                Dim brCombo As New ComboBox
                Dim defcheck As New CheckBox
                AddHandler defcheck.CheckStateChanged, AddressOf defcheck_CheckedStateChanged

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

                If item.SubItems(5).Text = "True" Then
                    defcheck.Checked = True
                Else
                    defcheck.Checked = False
                End If

                vcCombo.DropDownStyle = ComboBoxStyle.DropDownList
                brCombo.DropDownStyle = ComboBoxStyle.DropDownList
                defcheck.Height = item.Bounds.Height
                defcheck.Width = lvFileStreams.Columns(5).Width - 42
                vcCombo.Height = item.Bounds.Height
                vcCombo.Width = lvFileStreams.Columns(6).Width - 2
                brCombo.Height = item.Bounds.Height
                brCombo.Width = lvFileStreams.Columns(7).Width - 2
                defcheck.Location = New Point(lvFileStreams.Items(z).SubItems(4).Bounds.Right + 40, lvFileStreams.Items(z).SubItems(4).Bounds.Y)
                vcCombo.Location = New Point(lvFileStreams.Items(z).SubItems(5).Bounds.Right, lvFileStreams.Items(z).SubItems(5).Bounds.Y)
                brCombo.Location = New Point(lvFileStreams.Items(z).SubItems(6).Bounds.Right, lvFileStreams.Items(z).SubItems(6).Bounds.Y)
                vcCombo.Parent = lvFileStreams
                brCombo.Parent = lvFileStreams
                vcCombo.Name = "vcCombo" & z.ToString.Trim
                brCombo.Name = "brCombo" & z.ToString.Trim
                defcheck.Name = "defCheck" & z.ToString.Trim
                lvFileStreams.Controls.Add(defcheck)
                lvFileStreams.Controls.Add(vcCombo)
                lvFileStreams.Controls.Add(brCombo)

                lvFileStreams.Items(z).SubItems(5).Text = ""
                lvFileStreams.Items(z).SubItems(6).Text = ""
            Next z
        End If
    End Sub

    Private Sub vcCombo_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim vcCombo As ComboBox = sender
        If vcCombo.Name = "" Then Exit Sub
        Dim brC As String = "br" & Mid(vcCombo.Name, 3)
        Dim si As String = vcCombo.SelectedItem
        Dim typ As String = lvFileStreams.Items(CInt(Replace(vcCombo.Name.ToString, "vcCombo", ""))).SubItems(1).Text

        For Each ctrl In lvFileStreams.Controls.Find(brC, True)

            Dim brCombo As ComboBox = ctrl
            brCombo.Items.Clear()

            Select Case si
                Case "copy"
                    brCombo.Items.Add("------")
                    brCombo.SelectedIndex = 0
                    If typ = "Video" Then
                        With ComboBox5
                            .Items.Clear()
                            .Items.Add("------")
                            .SelectedIndex = 0
                            .Enabled = False
                        End With
                    End If

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
                    'h.264 Profile
                    With ComboBox5
                        .Items.Clear()
                        .Items.Add("Baseline")
                        .Items.Add("Main")
                        .Items.Add("High")
                        .Items.Add("High10")
                        .SelectedIndex = 2
                        .Enabled = True
                    End With
                    'h264 level
                    With ComboBox6
                        .Items.Clear()
                        .Items.Add("3")
                        .Items.Add("3.1")
                        .Items.Add("3.2")
                        .Items.Add("4")
                        .Items.Add("4.1")
                        .Items.Add("4.2")
                        .Items.Add("5")
                        .Items.Add("5.1")
                        .Items.Add("5.2")
                        .SelectedIndex = 5
                        .Enabled = True
                    End With

                Case "x265", "Intel QSV H.265", "NVidia NVenc H.265"
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
                    'profle h.265
                    With ComboBox5
                        .Items.Clear()
                        .Items.Add("Main")
                        .Items.Add("Main10")
                        .SelectedIndex = 0
                        .Enabled = True
                    End With
                    'level h.265
                    With ComboBox6
                        .Items.Clear()
                        .Items.Add("3")
                        .Items.Add("3.1")
                        .Items.Add("4")
                        .Items.Add("4.1")
                        .Items.Add("5")
                        .Items.Add("5.1")
                        .Items.Add("5.2")
                        .Items.Add("6")
                        .Items.Add("6.1")
                        .Items.Add("6.2")
                        .SelectedIndex = 4
                        .Enabled = True
                    End With

            End Select
            Exit For
        Next
    End Sub

    Private Sub defcheck_CheckedStateChanged(sender As Object, e As EventArgs)
        If lvFileStreams.Items.Count = 0 Then Exit Sub
        Dim AudioDefault As Integer = 0
        Dim UntertitelDefault As Integer = 0

        ' Prüfen op nur ein Default pro Streamtyp
        For z = 0 To lvFileStreams.Items.Count - 1
            For Each ctrl In lvFileStreams.Controls.Find("defCheck" & z.ToString.Trim, True)
                Dim defCheckBox As CheckBox = ctrl

                Select Case lvFileStreams.Items(z).SubItems(1).Text
                    Case "Video"

                    Case "Audio"
                        If AudioDefault = 0 And defCheckBox.Checked = True Then AudioDefault += 1 Else defCheckBox.Checked = False

                    Case "Untertitel"
                        If UntertitelDefault = 0 And defCheckBox.Checked = True Then UntertitelDefault += 1 Else defCheckBox.Checked = False

                End Select
            Next
        Next z
    End Sub

    Private Sub BeendenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeendenToolStripMenuItem.Click
        If WorkingList.BgWffmpeg.IsBusy = True Then
            WorkingList.BgWffmpeg.CancelAsync()
            Threading.Thread.Sleep(500)
        End If
        End
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If WorkingList.BgWffmpeg.IsBusy = True Then
            WorkingList.BgWffmpeg.CancelAsync()
            Threading.Thread.Sleep(500)
        End If
    End Sub

    Private Sub EinstellungenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EinstellungenToolStripMenuItem.Click
        Settings.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        WorkingList.Location = My.Settings.WorkingListPosition
        WorkingList.Show()
    End Sub

End Class

