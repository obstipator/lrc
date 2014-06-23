Imports System.Drawing.Drawing2D
Imports System.Text.RegularExpressions
Imports System.Drawing.Text


Public Class Form1

	Private Const WS_EX_TRANSPARENT As Integer = &H20

	Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
		Get
			Dim cp As CreateParams = MyBase.CreateParams
			cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT
			Return cp
		End Get
	End Property

	Private Sub iTunes_OnPlayerPlayEvent(ByVal iTrack As Object)
		MessageBox.Show("song diff!")
	End Sub


	Dim iTunesApp
	Dim track

	Dim lyrics

	Dim sWidth = 1920
	Dim sHeight = 1080

	Dim bHeight = 100


	Private canvas As Graphics


	Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Console.WriteLine("asdkghalkshgd")


		Me.SetBounds(0, sHeight - bHeight, sWidth, bHeight)

		'MessageBox.Show("oooo")
		iTunesApp = CreateObject("iTunes.Application")
		track = iTunesApp.CurrentTrack
		'Dim isMuted = iTunesApp.Mute

		'cv = Me.CreateGraphics()




		'Me.Lyric.ForeColor = Color.Transparent
		'Me.Lyric.Text = "Hellooo: " & playerPosition


		SetStyle(ControlStyles.SupportsTransparentBackColor, True)

		'SetStyle(ControlStyles.Opaque, False)

		Me.BackColor = Color.Black
		'Me.BackColor = Color.Transparent

		Me.Opacity = 0.5



		'Me.Lyric.BackColor = Color.Blue
		'Me.Lyric.Text = "aksdgl"


		'Me.BackColor = Color.FromArgb(Val("&H9900ff00"))

		'Me.ForeColor = Color.White
		'Me.ControlBox = False
		'Me.Text = ""

		'Me.Lyric.ForeColor = Color.Green

		'Me.Visible = True

		'cv.FillRectangle(Brushes.Green, 0, 0, 10, 10)

		''''MessageBox.Show("1111")

		Timer1.Start()
		resetTrack()


	End Sub


	Private currentLyric = "empty"
	Private playerPositionSeconds
	Private curOffsetMs


	Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
		'Dim cv As Graphics = e.Graphics

		''Dim canvas As Graphics = e.Graphics
		'If IsNothing(canvas) Then

		'	Console.WriteLine("nothin")
		'	Return
		'End If

		canvas = e.Graphics

		Dim curMillis = DateTime.Now.Millisecond



		'DateTime.Now.Millisecond

		'WriteText(currentLyric & " -- " & offsetMs & " --- " & curMillis)
		'WriteText((iTunesApp.PlayerPosition + (DateTime.Now.Millisecond / 1000.0) + offsetMs).ToString())


		If needsUpdate Then
			WriteText(currentLyric)
			needsUpdate = False
		End If


	End Sub


	Dim timeObj As New List(Of Double)
	Dim lyricObj As New List(Of String)

	Dim currentLyricId = 0
	'Dim nextLyricTimestamp = 0
	Dim earlyOffset = -0.5
	Dim needsUpdate = True

	Private Sub getCurrentLyric()

		Dim lyricLen = timeObj.Count

		Console.WriteLine("id: " & currentLyricId & ", len: " & lyricLen & ", ms: " & curOffsetMs)

		If currentLyricId + 1 >= lyricLen Then
			currentLyric = "END?"
			Return
		End If

		If timeObj.Item(currentLyricId) < curOffsetMs - earlyOffset Then
			currentLyricId += 1
			currentLyric = lyricObj.Item(currentLyricId - 1)
			getCurrentLyric()
			needsUpdate = True
			Console.WriteLine(currentLyric)

		End If

		'For i As Integer = 0 To lyricLen - 1 Step 1
		'	If timeObj.Item(i + 1) > curOffsetMs + earlyOffset Then
		'		currentLyric = lyricObj.Item(i)
		'		'Console.WriteLine("asdf" & currentLyric)
		'		Exit For
		'	End If
		'Next

	End Sub

	Private Sub parseLyrics()
		Console.WriteLine("test")
		'Console.WriteLine(track.lyrics)

		timeObj = New List(Of Double)
		lyricObj = New List(Of String)

		lyrics = track.lyrics

		Dim lines = Split(lyrics, Chr(10))
		Console.WriteLine(lines)

		Dim pattern As String = "\[(?<minutes>\d{2}):(?<seconds>\d{2})\.(?<dec>\d{2})\](?<lyric>.*?)$"

		Dim rx As New Regex(pattern, RegexOptions.Compiled Or RegexOptions.IgnoreCase)

		For Each el In lines
			Dim line = el.Trim()
			Dim matches As MatchCollection = rx.Matches(line)

			If matches.Count = 0 Then
				Continue For
			End If

			For Each Match As Match In matches
				Dim groups As GroupCollection = Match.Groups

				Dim minutes = groups.Item("minutes").Value
				Dim seconds = groups.Item("seconds").Value
				Dim dec = groups.Item("dec").Value


				Dim timestamp As Double = minutes * 60.0 + seconds + (dec / 100.0)
				Dim lyric = groups.Item("lyric").ToString()

				timeObj.Add(timestamp)
				lyricObj.Add(lyric)

			Next




		Next



	End Sub





	Private Sub WriteText(ByVal str As String)

		'canvas.Clear(Color.Transparent)

		canvas.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit


		'Dim brush = New SolidBrush(Color.FromArgb(Val("&H9900ff00")))
		'Dim p1 = New Point(0, 0)
		'Dim p2 = New Point(10, 10)
		'Dim b = New LinearGradientBrush(p1, p2, Color.FromArgb(Val("&H9900ff00")), Color.FromArgb(Val("&H3900ffff")))
		'canvas.FillRectangle(b, 0, 0, 100, 100)


		Dim drawString As [String] = str

		' Create font and brush. 
		Dim drawFont As New Font("TF2 Build", 40)
		'Dim drawBrush As New SolidBrush(Color.FromArgb(Val("&Hff010101")))
		Dim drawBrush As New SolidBrush(Color.White)


		Dim blackBrush As New SolidBrush(Color.FromArgb(&HFF010101))

		' Create rectangle for drawing. 
		Dim x As Single = 0.0F
		Dim y As Single = 0.0F
		Dim width As Single = Single.Parse(sWidth)
		Dim height As Single = Single.Parse(sHeight)
		Dim drawRect As New RectangleF(x, y, width, height)

		' Draw rectangle to screen. 
		'Dim blackPen As New Pen(Color.Red)
		'canvas.DrawRectangle(blackPen, x, y, width, height)

		' Set format of string. 
		Dim drawFormat As New StringFormat
		drawFormat.Alignment = StringAlignment.Center

		' Draw string to screen.

		Dim dist = 2

		Dim strokePos(,) As Integer = New Integer(8, 1) {{x - dist, y - dist}, {x - dist, y + dist}, {x - dist, y}, {x, y - dist}, {x, y + dist}, {x, y}, {x + dist, y - dist}, {x + dist, y + dist}, {x + dist, y}}
		For i = 0 To 8
			Dim blackRect As New RectangleF(strokePos(i, 0), strokePos(i, 1), width, height)
			canvas.DrawString(drawString, drawFont, blackBrush, blackRect, drawFormat)
		Next

		Dim dist2 = 2

		Dim strokePos2(,) As Integer = New Integer(8, 1) {{x - dist2, y - dist2}, {x - dist2, y + dist2}, {x - dist2, y}, {x, y - dist2}, {x, y + dist2}, {x, y}, {x + dist2, y - dist2}, {x + dist2, y + dist2}, {x + dist2, y}}
		For i = 0 To 8
			Dim blackRect As New RectangleF(strokePos2(i, 0), strokePos2(i, 1), width, height)
			canvas.DrawString(drawString, drawFont, blackBrush, blackRect, drawFormat)
		Next




		canvas.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat)
		'MessageBox.Show("ffff")

	End Sub

	Dim timeBounds = {0, 0, 2}

	Dim currentTrackId = 0


	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		'Console.WriteLine(track.Location)

		If currentTrackId <> iTunesApp.CurrentTrack.trackID Then
			'Console.WriteLine(1)

			resetTrack()
		End If



		Dim playerPosition = iTunesApp.PlayerPosition

		playerPositionSeconds = playerPosition

		curOffsetMs = playerPositionSeconds + (DateTime.Now.Millisecond + offsetMs Mod 1000) / 1000.0

		getCurrentLyric()

		If needsUpdate Then
			Me.Invalidate()
		End If

	End Sub



	Dim startSeconds = 0.0
	Dim endOffset = 0.0
	Dim offsetMs = 0.0
	Dim startMillis = 0.0

	Private Sub CalcMilliOffset()
		startSeconds = iTunesApp.PlayerPosition
		curOffset = 0.0

		startMillis = DateTime.Now.Millisecond
		TimerCalcMs.Start()
	End Sub


	Dim curOffset = 0.0

	Private Sub TimerCalcMs_Tick(sender As Object, e As EventArgs) Handles TimerCalcMs.Tick

		Dim playerPosition = iTunesApp.PlayerPosition


		If (playerPosition >= startSeconds + 1) Then
			TimerCalcMs.Stop()

			Dim currentMillis = DateTime.Now.Millisecond

			If playerPosition = startSeconds + 1 Then
				offsetMs = (((currentMillis + 1000) - startMillis) Mod 1000) / 1000.0
			End If

		End If
	End Sub


	Private Sub resetTrack()
		Console.WriteLine("RESETING")
		track = iTunesApp.CurrentTrack
		currentTrackId = track.trackID

		CalcMilliOffset()
		parseLyrics()

		If lyricObj.Count = 0 Then
			FormText.Hide()
		Else
			Me.Hide()
			FormText.Show()
			Me.Show()
		End If

		currentLyricId = 0
		currentLyric = ""

		needsUpdate = True
		Me.Invalidate()
	End Sub



End Class
