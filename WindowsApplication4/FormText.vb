Public Class FormText

	Private Const WS_EX_TRANSPARENT As Integer = &H20

	Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
		Get
			Dim cp As CreateParams = MyBase.CreateParams
			cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT
			Return cp
		End Get
	End Property

	Private Sub FormText_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Me.Opacity = 0.2
		Dim sWidth = 1920
		Dim sHeight = 1080

		Dim bHeight = 100

		Me.SetBounds(0, sHeight - bHeight, sWidth, bHeight)


	End Sub
End Class