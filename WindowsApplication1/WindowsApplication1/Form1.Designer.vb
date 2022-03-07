<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnClickThis = New System.Windows.Forms.Button()
        Me.lbHelloWorld = New System.Windows.Forms.Label()
        Me.SuspendLayout
        '
        'btnClickThis
        '
        Me.btnClickThis.Location = New System.Drawing.Point(7, 26)
        Me.btnClickThis.Name = "btnClickThis"
        Me.btnClickThis.Size = New System.Drawing.Size(135, 53)
        Me.btnClickThis.TabIndex = 0
        Me.btnClickThis.Text = "click"
        Me.btnClickThis.UseVisualStyleBackColor = true
        '
        'lbHelloWorld
        '
        Me.lbHelloWorld.AutoSize = true
        Me.lbHelloWorld.Location = New System.Drawing.Point(54, 129)
        Me.lbHelloWorld.Name = "lbHelloWorld"
        Me.lbHelloWorld.Size = New System.Drawing.Size(38, 12)
        Me.lbHelloWorld.TabIndex = 1
        Me.lbHelloWorld.Text = "Label1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 12!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.lbHelloWorld)
        Me.Controls.Add(Me.btnClickThis)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents btnClickThis As System.Windows.Forms.Button
    Friend WithEvents lbHelloWorld As System.Windows.Forms.Label

End Class
