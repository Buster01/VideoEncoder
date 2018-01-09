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
        ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden
        ProcessProperties.CreateNoWindow = True
        ProcessProperties.Arguments = ffprobe_arguments

        ffprobeProcess = Process.Start(ProcessProperties)
        out_tmp = ffprobeProcess.StandardOutput.ReadToEnd()
        stderr = ffprobeProcess.StandardError.ReadToEnd()

        If String.IsNullOrEmpty(stderr) = False Then Return 0
        Dim stdout As Xml.XmlDocument = New Xml.XmlDocument()
        stdout.LoadXml(out_tmp)

        Dim cnode As Xml.XmlNode = stdout.SelectSingleNode("/ffprobe/format")
        duration = CDbl(Replace(cnode.Attributes.GetNamedItem("duration").Value, ".", ","))

        Return duration
    End Function

    Public Function VideoFileStreams(file As String, ffmpeg_path As String) As Xml.XmlNode
        Dim ProcessProperties As New ProcessStartInfo
        Dim ffprobeProcess As New Process

        Dim duration As Double = 0
        Dim ffprobe_arguments As String = " -v error -show_streams -of xml  -i " & Chr(34) & file & Chr(34)

        Dim out_tmp As String = ""
        Dim stderr As String = ""


        ProcessProperties.FileName = ffmpeg_path & "ffprobe.exe"
        ProcessProperties.WorkingDirectory = ffmpeg_path
        ProcessProperties.UseShellExecute = False
        ProcessProperties.RedirectStandardOutput = True
        ProcessProperties.RedirectStandardError = True
        ProcessProperties.WindowStyle = ProcessWindowStyle.Normal
        ProcessProperties.Arguments = ffprobe_arguments
        ProcessProperties.CreateNoWindow = True

        ffprobeProcess = Process.Start(ProcessProperties)
        out_tmp = ffprobeProcess.StandardOutput.ReadToEnd()
        stderr = ffprobeProcess.StandardError.ReadToEnd()

        If Len(out_tmp) <= 10 Then Return Nothing
        Dim stdout As Xml.XmlDocument = New Xml.XmlDocument()
        stdout.LoadXml(out_tmp)

        Dim cnode As Xml.XmlNode = stdout.SelectSingleNode("/ffprobe/streams")
        Return cnode

    End Function

    Public Function XMLStreamAnalyse(ByVal stream As Xml.XmlNode) As ListViewItem
        Dim StreamData(6) As String

        'Stream Index
        StreamData(0) = stream.Attributes("index").Value

        ' Stream Typ ermitteln
        If stream.Attributes.ItemOf("codec_type") IsNot Nothing Then
            Select Case stream.Attributes("codec_type").Value
                Case "video"
                    StreamData(1) = "Video"

                Case "audio"
                    StreamData(1) = "Audio"

                Case "subtitle"
                    StreamData(1) = "Untertitel"

            End Select
        End If

        'Stream Codec ermitteln
        If stream.Attributes.ItemOf("codec_name") IsNot Nothing Then
            Select Case stream.Attributes("codec_name").Value
                Case "h264"
                    StreamData(2) = "H.264"

                Case "hevc"
                    StreamData(2) = "H.265"

                Case "mpeg2video"
                    StreamData(2) = "MPEG2"

                Case "dvb_teletext"
                    StreamData(2) = "VideoText"

                Case "mp2"
                    StreamData(2) = "MP2 Audio"

                Case "ac3"
                    StreamData(2) = "AC-3"

                Case "aac"
                    StreamData(2) = "AAC"

                Case "dts"
                    StreamData(2) = "DTS"

                Case "truehd"
                    StreamData(2) = "DTS TrueHD"

                Case "hdmv_pgs_subtitle"
                    StreamData(2) = "PGS"

                Case "dvd_subtitle"
                    StreamData(2) = "DVD"

            End Select
        End If

        'Audio Layout ermitteln
        If stream.Attributes.ItemOf("channel_layout") IsNot Nothing Then
            Select Case stream.Attributes("channel_layout").Value
                Case "mono"
                    StreamData(2) = StreamData(2) & " (Mono)"

                Case "stereo"
                    StreamData(2) = StreamData(2) & " (Stereo)"

                Case "5.1(side)"
                    StreamData(2) = StreamData(2) & " (5.1)"

                Case "6.1"
                    StreamData(2) = StreamData(2) & " (6.1)"

                Case "7.1"
                    StreamData(2) = StreamData(2) & " (7.1)"
            End Select
        End If

        'Stream Sprache ermitteln
        For Each tag As Xml.XmlNode In stream.ChildNodes
            If tag.Attributes.ItemOf("key") IsNot Nothing Then
                If tag.Attributes("key").Value.ToLower = "language" Then
                    StreamData(3) = tag.Attributes("value").Value
                End If
            End If
        Next

        'Number of Frames
        For Each tag As Xml.XmlNode In stream.ChildNodes
            If tag.Attributes.ItemOf("key") IsNot Nothing Then
                If tag.Attributes("key").Value.ToLower = "number_of_frames-eng" Then
                    StreamData(4) = Format(Val(tag.Attributes("value").Value), "#,##0")
                End If
            End If
        Next

        'default Stream
        For Each tag As Xml.XmlNode In stream.ChildNodes
            If tag.Attributes.ItemOf("default") IsNot Nothing Then
                If tag.Attributes("default").Value = "1" Then StreamData(5) = "True" Else StreamData(5) = "False"
            End If
        Next

        Return New ListViewItem(StreamData)
    End Function
End Module
