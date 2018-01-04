Module VideoEncoding
    Public Function VideoDuration(file As String, ffmpeg_path As String) As Long
        ' Ermittelt die länge der Inputdatei in Sekunden

        Dim ProcessProperties As New ProcessStartInfo
        Dim ffprobeProcess As New Process

        Dim duration As Double = 0
        Dim ffprobe_arguments As String = " -v error -show_format -of xml  -i " & Chr(34) & file & Chr(34)

        Dim out_tmp As String = ""
        Dim stderr As String = ""


        ProcessProperties.FileName = ffmpeg_path & "ffprobe.exe"
        ProcessProperties.WorkingDirectory = ffmpeg_path
        ProcessProperties.UseShellExecute = False
        ProcessProperties.RedirectStandardOutput = True
        ProcessProperties.RedirectStandardError = True
        ProcessProperties.WindowStyle = ProcessWindowStyle.Normal
        ProcessProperties.Arguments = ffprobe_arguments

        ffprobeProcess = Process.Start(ProcessProperties)
        out_tmp = ffprobeProcess.StandardOutput.ReadToEnd()
        stderr = ffprobeProcess.StandardError.ReadToEnd()

        If String.IsNullOrEmpty(stderr) = False Then Return 0
        Dim stdout As Xml.XmlDocument = New Xml.XmlDocument()
        stdout.LoadXml(out_tmp)
        Dim cnode As Xml.XmlNode = stdout.ChildNodes(1).ChildNodes(0)
        duration = CDbl(Replace(cnode.Attributes.GetNamedItem("duration").Value, ".", ","))

        Return duration
    End Function
End Module
