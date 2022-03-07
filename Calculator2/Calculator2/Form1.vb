Public Class Form1

    '今回押されたボタンの数字以外の値(+,-,c,=)を保存する変数
    Private ope As String = Nothing
    '今回押されたボタンの値(1~0,+,-,c)を保存する変数
    Private beforeIn As String = "C"
    '演算結果を保存する変数
    Private ans As Long = 0
    '今回入力された数値を保存する変数
    Private num As Long = 0

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles LblAnser.Click

    End Sub

    Private Sub ButtonOne_Click(sender As Object, e As EventArgs) Handles ButtonOne.Click
        If beforeIn >= "1" And beforeIn <= "3" Then
            '直前の押されたのが数字ボタンのとき
            'かつ、8桁未満の数値が入力されているとき
            If LblAnser.Text.Length < 8 Then

            End If
        End If
    End Sub

    Private Sub ButtonTwo_Click(sender As Object, e As EventArgs) Handles ButtonTwo.Click
        LblAnser.Text = LblAnser.Text & "2"
    End Sub

    Private Sub ButtonThree_Click(sender As Object, e As EventArgs) Handles ButtonThree.Click
        LblAnser.Text = LblAnser.Text & "3"
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ButtonZero_Click(sender As Object, e As EventArgs) Handles ButtonZero.Click
        LblAnser.Text = LblAnser.Text & "0"

    End Sub

    Private Sub Buttonfour_Click(sender As Object, e As EventArgs) Handles Buttonfour.Click
        LblAnser.Text = LblAnser.Text & "4"

    End Sub

    Private Sub Buttonfive_Click(sender As Object, e As EventArgs) Handles Buttonfive.Click
        LblAnser.Text = LblAnser.Text & "5"

    End Sub

    Private Sub ButtonSix_Click(sender As Object, e As EventArgs) Handles ButtonSix.Click
        LblAnser.Text = LblAnser.Text & "6"

    End Sub

    Private Sub ButtonSeven_Click(sender As Object, e As EventArgs) Handles ButtonSeven.Click
        LblAnser.Text = LblAnser.Text & "7"

    End Sub

    Private Sub ButtonEight_Click(sender As Object, e As EventArgs) Handles ButtonEight.Click
        LblAnser.Text = LblAnser.Text & "8"

    End Sub

    Private Sub ButtonNine_Click(sender As Object, e As EventArgs) Handles ButtonNine.Click
        LblAnser.Text = LblAnser.Text & "9"

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Buttonplus.Click

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles ButtonClear.Click

    End Sub

    Private Sub Button1Equal_Click(sender As Object, e As EventArgs) Handles Button1Equal.Click

    End Sub
End Class
