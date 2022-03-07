Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Year関数で今日の西暦年号を取得 
        Dim seireki As Integer = Year(Now)
        ' Month関数で今日の月を取得
        Dim tsuki As Integer = Month(Now)
        ' day関数で今日の日付を取得
        Dim hi As Integer = DateAndTime.Day(Now)
        ' Hour関数で現在の時を取得
        Dim ji As Integer = Hour(Now)
        ' Minute関数で現在の分を取得
        Dim hun As Integer = Minute(Now)
        ' Sekond関数で現在の秒を取得
        Dim Byou As Integer = Second(Now)

        ' ラベルLblNowのTextプロパティに代入
        LblNow.Text = seireki & "年"
        LblNow.Text &= tsuki & "月"
        LblNow.Text &= hi & "日"
        LblNow.Text &= ji & "時"
        LblNow.Text &= hun & "分"
        LblNow.Text &= Byou & "秒"

    End Sub
End Class
