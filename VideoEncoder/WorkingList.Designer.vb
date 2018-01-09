<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WorkingList
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.StartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PauseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FertigeAufträgeEntfernenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AlleAufträgeEntfernenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ListView1)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(886, 549)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ListeToolStripMenuItem, Me.StartToolStripMenuItem, Me.PauseToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(899, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ListView1
        '
        Me.ListView1.Location = New System.Drawing.Point(6, 19)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(874, 523)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'StartToolStripMenuItem
        '
        Me.StartToolStripMenuItem.Name = "StartToolStripMenuItem"
        Me.StartToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.StartToolStripMenuItem.Text = "Start"
        '
        'PauseToolStripMenuItem
        '
        Me.PauseToolStripMenuItem.Name = "PauseToolStripMenuItem"
        Me.PauseToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.PauseToolStripMenuItem.Text = "Pause"
        '
        'ListeToolStripMenuItem
        '
        Me.ListeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FertigeAufträgeEntfernenToolStripMenuItem, Me.AlleAufträgeEntfernenToolStripMenuItem})
        Me.ListeToolStripMenuItem.Name = "ListeToolStripMenuItem"
        Me.ListeToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.ListeToolStripMenuItem.Text = "Liste"
        '
        'FertigeAufträgeEntfernenToolStripMenuItem
        '
        Me.FertigeAufträgeEntfernenToolStripMenuItem.Name = "FertigeAufträgeEntfernenToolStripMenuItem"
        Me.FertigeAufträgeEntfernenToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.FertigeAufträgeEntfernenToolStripMenuItem.Text = "Fertige Aufträge entfernen"
        '
        'AlleAufträgeEntfernenToolStripMenuItem
        '
        Me.AlleAufträgeEntfernenToolStripMenuItem.Name = "AlleAufträgeEntfernenToolStripMenuItem"
        Me.AlleAufträgeEntfernenToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.AlleAufträgeEntfernenToolStripMenuItem.Text = "Alle Aufträge entfernen"
        '
        'WorkingList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 581)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "WorkingList"
        Me.Text = "VideoEncoder Aufgabenliste"
        Me.GroupBox1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ListView1 As ListView
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ListeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FertigeAufträgeEntfernenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AlleAufträgeEntfernenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PauseToolStripMenuItem As ToolStripMenuItem
End Class
