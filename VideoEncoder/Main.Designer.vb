﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbFiles = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblInputDirectory = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.EinstellungenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeendenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblOutputDirectory = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog2 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbQualitaet = New System.Windows.Forms.ComboBox()
        Me.cbDeInterlace = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.ComboBox6 = New System.Windows.Forms.ComboBox()
        Me.ComboBox5 = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lvFileStreams = New System.Windows.Forms.ListView()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.FSW_Inputdir = New System.IO.FileSystemWatcher()
        Me.cbEncPresets = New System.Windows.Forms.ComboBox()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.FSW_Inputdir, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cbFiles)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.lblInputDirectory)
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(752, 80)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Quelle"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(469, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Info"
        '
        'cbFiles
        '
        Me.cbFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFiles.FormattingEnabled = True
        Me.cbFiles.Location = New System.Drawing.Point(24, 50)
        Me.cbFiles.Name = "cbFiles"
        Me.cbFiles.Size = New System.Drawing.Size(425, 21)
        Me.cbFiles.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(24, 18)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(38, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "...."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblInputDirectory
        '
        Me.lblInputDirectory.AutoSize = True
        Me.lblInputDirectory.Location = New System.Drawing.Point(68, 23)
        Me.lblInputDirectory.Name = "lblInputDirectory"
        Me.lblInputDirectory.Size = New System.Drawing.Size(32, 13)
        Me.lblInputDirectory.TabIndex = 1
        Me.lblInputDirectory.Text = "Pfad:"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Checked = True
        Me.CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox1.Location = New System.Drawing.Point(633, 22)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(112, 17)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.Text = "Ordner auswählen"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(764, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EinstellungenToolStripMenuItem, Me.BeendenToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(46, 20)
        Me.ToolStripMenuItem1.Text = "Datei"
        '
        'EinstellungenToolStripMenuItem
        '
        Me.EinstellungenToolStripMenuItem.Name = "EinstellungenToolStripMenuItem"
        Me.EinstellungenToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.EinstellungenToolStripMenuItem.Text = "Einstellungen"
        '
        'BeendenToolStripMenuItem
        '
        Me.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem"
        Me.BeendenToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.BeendenToolStripMenuItem.Text = "Beenden"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblOutputDirectory)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 114)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(752, 64)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Ziel"
        '
        'lblOutputDirectory
        '
        Me.lblOutputDirectory.AutoSize = True
        Me.lblOutputDirectory.Location = New System.Drawing.Point(68, 30)
        Me.lblOutputDirectory.Name = "lblOutputDirectory"
        Me.lblOutputDirectory.Size = New System.Drawing.Size(32, 13)
        Me.lblOutputDirectory.TabIndex = 1
        Me.lblOutputDirectory.Text = "Pfad:"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(24, 25)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(34, 23)
        Me.Button2.TabIndex = 0
        Me.Button2.Text = "...."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(603, 485)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(155, 29)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "zur Auftragsliste hinzufügen"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cbEncPresets)
        Me.GroupBox5.Controls.Add(Me.Label3)
        Me.GroupBox5.Controls.Add(Me.cbQualitaet)
        Me.GroupBox5.Controls.Add(Me.cbDeInterlace)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.Controls.Add(Me.CheckBox4)
        Me.GroupBox5.Controls.Add(Me.CheckBox3)
        Me.GroupBox5.Controls.Add(Me.ComboBox6)
        Me.GroupBox5.Controls.Add(Me.ComboBox5)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 385)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(752, 94)
        Me.GroupBox5.TabIndex = 8
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Video Optionen"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(242, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Qualität:"
        '
        'cbQualitaet
        '
        Me.cbQualitaet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbQualitaet.FormattingEnabled = True
        Me.cbQualitaet.Location = New System.Drawing.Point(295, 54)
        Me.cbQualitaet.Name = "cbQualitaet"
        Me.cbQualitaet.Size = New System.Drawing.Size(178, 21)
        Me.cbQualitaet.TabIndex = 13
        '
        'cbDeInterlace
        '
        Me.cbDeInterlace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDeInterlace.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDeInterlace.FormattingEnabled = True
        Me.cbDeInterlace.Location = New System.Drawing.Point(104, 54)
        Me.cbDeInterlace.Name = "cbDeInterlace"
        Me.cbDeInterlace.Size = New System.Drawing.Size(121, 23)
        Me.cbDeInterlace.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Deinterlace:"
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Location = New System.Drawing.Point(532, 57)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(82, 17)
        Me.CheckBox4.TabIndex = 10
        Me.CheckBox4.Text = "DTS 7.1 Fix"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Checked = True
        Me.CheckBox3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox3.Location = New System.Drawing.Point(629, 57)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(116, 17)
        Me.CheckBox3.TabIndex = 9
        Me.CheckBox3.Text = "Hardware Decoder"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'ComboBox6
        '
        Me.ComboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox6.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox6.FormattingEnabled = True
        Me.ComboBox6.Location = New System.Drawing.Point(295, 19)
        Me.ComboBox6.Name = "ComboBox6"
        Me.ComboBox6.Size = New System.Drawing.Size(51, 23)
        Me.ComboBox6.TabIndex = 7
        '
        'ComboBox5
        '
        Me.ComboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox5.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox5.FormattingEnabled = True
        Me.ComboBox5.Location = New System.Drawing.Point(104, 19)
        Me.ComboBox5.Name = "ComboBox5"
        Me.ComboBox5.Size = New System.Drawing.Size(121, 23)
        Me.ComboBox5.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(242, 23)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(36, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Level:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(21, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(33, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Profil:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lvFileStreams)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 184)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(752, 195)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Stream Optionen"
        '
        'lvFileStreams
        '
        Me.lvFileStreams.HideSelection = False
        Me.lvFileStreams.HoverSelection = True
        Me.lvFileStreams.Location = New System.Drawing.Point(24, 19)
        Me.lvFileStreams.Name = "lvFileStreams"
        Me.lvFileStreams.Size = New System.Drawing.Size(721, 170)
        Me.lvFileStreams.TabIndex = 0
        Me.lvFileStreams.UseCompatibleStateImageBehavior = False
        Me.lvFileStreams.View = System.Windows.Forms.View.Details
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(6, 485)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(138, 29)
        Me.Button4.TabIndex = 11
        Me.Button4.Text = "Auftragsliste anzeigen"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'FSW_Inputdir
        '
        Me.FSW_Inputdir.EnableRaisingEvents = True
        Me.FSW_Inputdir.SynchronizingObject = Me
        '
        'cbEncPresets
        '
        Me.cbEncPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEncPresets.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbEncPresets.FormattingEnabled = True
        Me.cbEncPresets.Location = New System.Drawing.Point(352, 20)
        Me.cbEncPresets.Name = "cbEncPresets"
        Me.cbEncPresets.Size = New System.Drawing.Size(121, 23)
        Me.cbEncPresets.TabIndex = 14
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(764, 521)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Video Encoder"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.FSW_Inputdir, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents lblInputDirectory As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents EinstellungenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BeendenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblOutputDirectory As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents FolderBrowserDialog2 As FolderBrowserDialog
    Friend WithEvents Button3 As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents ComboBox6 As ComboBox
    Friend WithEvents ComboBox5 As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cbFiles As ComboBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lvFileStreams As ListView
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents CheckBox4 As CheckBox
    Friend WithEvents cbDeInterlace As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button4 As Button
    Friend WithEvents FSW_Inputdir As IO.FileSystemWatcher
    Friend WithEvents Label3 As Label
    Friend WithEvents cbQualitaet As ComboBox
    Friend WithEvents cbEncPresets As ComboBox
End Class
