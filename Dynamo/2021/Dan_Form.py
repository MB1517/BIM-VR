
import System.Drawing
import System.Windows.Forms

from System.Drawing import *
from System.Windows.Forms import *

class Form1(Form):
	def __init__(self):
		self.InitializeComponent()
	
	def InitializeComponent(self):
		self._groupBox_datum = System.Windows.Forms.GroupBox()
		self._groupBox_coordinates = System.Windows.Forms.GroupBox()
		self._groupBox_style = System.Windows.Forms.GroupBox()
		self._radioButton_XYZ = System.Windows.Forms.RadioButton()
		self._radioButton_TBH = System.Windows.Forms.RadioButton()
		self._radioButton_LWH = System.Windows.Forms.RadioButton()
		self._radioButton_mm = System.Windows.Forms.RadioButton()
		self._radioButton_cm = System.Windows.Forms.RadioButton()
		self._groupBox_place_text = System.Windows.Forms.GroupBox()
		self._radioButton_automatic = System.Windows.Forms.RadioButton()
		self._radioButton_manual = System.Windows.Forms.RadioButton()
		self._button_ok = System.Windows.Forms.Button()
		self._button_cancel = System.Windows.Forms.Button()
		self._panel1 = System.Windows.Forms.Panel()
		self._radio_top_left = System.Windows.Forms.RadioButton()
		self._radio_bottom_left = System.Windows.Forms.RadioButton()
		self._radio_bottom_right = System.Windows.Forms.RadioButton()
		self._radio_top_right = System.Windows.Forms.RadioButton()
		self._groupBox_datum.SuspendLayout()
		self._groupBox_coordinates.SuspendLayout()
		self._groupBox_style.SuspendLayout()
		self._groupBox_place_text.SuspendLayout()
		self.SuspendLayout()
		# 
		# groupBox_datum
		# 
		self._groupBox_datum.Controls.Add(self._radio_bottom_right)
		self._groupBox_datum.Controls.Add(self._radio_top_right)
		self._groupBox_datum.Controls.Add(self._radio_bottom_left)
		self._groupBox_datum.Controls.Add(self._radio_top_left)
		self._groupBox_datum.Controls.Add(self._panel1)
		self._groupBox_datum.Location = System.Drawing.Point(12, 12)
		self._groupBox_datum.Name = "groupBox_datum"
		self._groupBox_datum.Padding = System.Windows.Forms.Padding(30)
		self._groupBox_datum.Size = System.Drawing.Size(174, 164)
		self._groupBox_datum.TabIndex = 0
		self._groupBox_datum.TabStop = False
		self._groupBox_datum.Text = "Datum Location"
		# 
		# groupBox_coordinates
		# 
		self._groupBox_coordinates.Controls.Add(self._radioButton_LWH)
		self._groupBox_coordinates.Controls.Add(self._radioButton_TBH)
		self._groupBox_coordinates.Controls.Add(self._radioButton_XYZ)
		self._groupBox_coordinates.Location = System.Drawing.Point(192, 12)
		self._groupBox_coordinates.Name = "groupBox_coordinates"
		self._groupBox_coordinates.Padding = System.Windows.Forms.Padding(10)
		self._groupBox_coordinates.Size = System.Drawing.Size(101, 92)
		self._groupBox_coordinates.TabIndex = 1
		self._groupBox_coordinates.TabStop = False
		self._groupBox_coordinates.Text = "Coordinates"
		# 
		# groupBox_style
		# 
		self._groupBox_style.Controls.Add(self._radioButton_cm)
		self._groupBox_style.Controls.Add(self._radioButton_mm)
		self._groupBox_style.Location = System.Drawing.Point(192, 104)
		self._groupBox_style.Name = "groupBox_style"
		self._groupBox_style.Padding = System.Windows.Forms.Padding(10)
		self._groupBox_style.Size = System.Drawing.Size(101, 72)
		self._groupBox_style.TabIndex = 2
		self._groupBox_style.TabStop = False
		self._groupBox_style.Text = "Style"
		# 
		# radioButton_XYZ
		# 
		self._radioButton_XYZ.Checked = True
		self._radioButton_XYZ.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_XYZ.Location = System.Drawing.Point(10, 23)
		self._radioButton_XYZ.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_XYZ.Name = "radioButton_XYZ"
		self._radioButton_XYZ.Size = System.Drawing.Size(81, 20)
		self._radioButton_XYZ.TabIndex = 1
		self._radioButton_XYZ.TabStop = True
		self._radioButton_XYZ.Text = "XYZ"
		self._radioButton_XYZ.UseVisualStyleBackColor = True
		# 
		# radioButton_TBH
		# 
		self._radioButton_TBH.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_TBH.Location = System.Drawing.Point(10, 43)
		self._radioButton_TBH.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_TBH.Name = "radioButton_TBH"
		self._radioButton_TBH.Size = System.Drawing.Size(81, 20)
		self._radioButton_TBH.TabIndex = 2
		self._radioButton_TBH.Text = "TBH"
		self._radioButton_TBH.UseVisualStyleBackColor = True
		# 
		# radioButton_LWH
		# 
		self._radioButton_LWH.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_LWH.Location = System.Drawing.Point(10, 63)
		self._radioButton_LWH.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_LWH.Name = "radioButton_LWH"
		self._radioButton_LWH.Size = System.Drawing.Size(81, 20)
		self._radioButton_LWH.TabIndex = 3
		self._radioButton_LWH.Text = "LWH"
		self._radioButton_LWH.UseVisualStyleBackColor = True
		# 
		# radioButton_mm
		# 
		self._radioButton_mm.Checked = True
		self._radioButton_mm.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_mm.Location = System.Drawing.Point(10, 23)
		self._radioButton_mm.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_mm.Name = "radioButton_mm"
		self._radioButton_mm.Size = System.Drawing.Size(81, 20)
		self._radioButton_mm.TabIndex = 1
		self._radioButton_mm.TabStop = True
		self._radioButton_mm.Text = "millimeters"
		self._radioButton_mm.UseVisualStyleBackColor = True
		# 
		# radioButton_cm
		# 
		self._radioButton_cm.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_cm.Location = System.Drawing.Point(10, 43)
		self._radioButton_cm.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_cm.Name = "radioButton_cm"
		self._radioButton_cm.Size = System.Drawing.Size(81, 20)
		self._radioButton_cm.TabIndex = 2
		self._radioButton_cm.Text = "centimeters"
		self._radioButton_cm.UseVisualStyleBackColor = True
		# 
		# groupBox_place_text
		# 
		self._groupBox_place_text.Controls.Add(self._radioButton_manual)
		self._groupBox_place_text.Controls.Add(self._radioButton_automatic)
		self._groupBox_place_text.Location = System.Drawing.Point(192, 177)
		self._groupBox_place_text.Name = "groupBox_place_text"
		self._groupBox_place_text.Padding = System.Windows.Forms.Padding(10)
		self._groupBox_place_text.Size = System.Drawing.Size(101, 73)
		self._groupBox_place_text.TabIndex = 3
		self._groupBox_place_text.TabStop = False
		self._groupBox_place_text.Text = "Place Text"
		# 
		# radioButton_automatic
		# 
		self._radioButton_automatic.Checked = True
		self._radioButton_automatic.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_automatic.Location = System.Drawing.Point(10, 23)
		self._radioButton_automatic.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_automatic.Name = "radioButton_automatic"
		self._radioButton_automatic.Size = System.Drawing.Size(81, 20)
		self._radioButton_automatic.TabIndex = 2
		self._radioButton_automatic.TabStop = True
		self._radioButton_automatic.Text = "Automatic"
		self._radioButton_automatic.UseVisualStyleBackColor = True
		# 
		# radioButton_manual
		# 
		self._radioButton_manual.Dock = System.Windows.Forms.DockStyle.Top
		self._radioButton_manual.Location = System.Drawing.Point(10, 43)
		self._radioButton_manual.Margin = System.Windows.Forms.Padding(0)
		self._radioButton_manual.Name = "radioButton_manual"
		self._radioButton_manual.Size = System.Drawing.Size(81, 20)
		self._radioButton_manual.TabIndex = 3
		self._radioButton_manual.Text = "Manual"
		self._radioButton_manual.UseVisualStyleBackColor = True
		# 
		# button_ok
		# 
		self._button_ok.DialogResult = System.Windows.Forms.DialogResult.OK
		self._button_ok.Location = System.Drawing.Point(19, 226)
		self._button_ok.Name = "button_ok"
		self._button_ok.Size = System.Drawing.Size(75, 23)
		self._button_ok.TabIndex = 4
		self._button_ok.Text = "OK"
		self._button_ok.UseVisualStyleBackColor = True
		# 
		# button_cancel
		# 
		self._button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		self._button_cancel.Location = System.Drawing.Point(100, 226)
		self._button_cancel.Name = "button_cancel"
		self._button_cancel.Size = System.Drawing.Size(75, 23)
		self._button_cancel.TabIndex = 5
		self._button_cancel.Text = "Cancel"
		self._button_cancel.UseVisualStyleBackColor = True
		# 
		# panel1
		# 
		self._panel1.BackColor = System.Drawing.Color.White
		self._panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		self._panel1.Dock = System.Windows.Forms.DockStyle.Fill
		self._panel1.Location = System.Drawing.Point(30, 43)
		self._panel1.Name = "panel1"
		self._panel1.Size = System.Drawing.Size(114, 91)
		self._panel1.TabIndex = 0
		# 
		# radio_top_left
		# 
		self._radio_top_left.Location = System.Drawing.Point(14, 28)
		self._radio_top_left.Name = "radio_top_left"
		self._radio_top_left.Size = System.Drawing.Size(16, 16)
		self._radio_top_left.TabIndex = 0
		self._radio_top_left.TabStop = True
		self._radio_top_left.UseVisualStyleBackColor = True
		# 
		# radio_bottom_left
		# 
		self._radio_bottom_left.Location = System.Drawing.Point(14, 134)
		self._radio_bottom_left.Name = "radio_bottom_left"
		self._radio_bottom_left.Size = System.Drawing.Size(16, 16)
		self._radio_bottom_left.TabIndex = 1
		self._radio_bottom_left.TabStop = True
		self._radio_bottom_left.UseVisualStyleBackColor = True
		# 
		# radio_bottom_right
		# 
		self._radio_bottom_right.Location = System.Drawing.Point(147, 134)
		self._radio_bottom_right.Name = "radio_bottom_right"
		self._radio_bottom_right.Size = System.Drawing.Size(16, 16)
		self._radio_bottom_right.TabIndex = 3
		self._radio_bottom_right.TabStop = True
		self._radio_bottom_right.UseVisualStyleBackColor = True
		# 
		# radio_top_right
		# 
		self._radio_top_right.Location = System.Drawing.Point(147, 28)
		self._radio_top_right.Name = "radio_top_right"
		self._radio_top_right.Size = System.Drawing.Size(16, 16)
		self._radio_top_right.TabIndex = 2
		self._radio_top_right.TabStop = True
		self._radio_top_right.UseVisualStyleBackColor = True
		# 
		# Form1
		# 
		self.AcceptButton = self._button_ok
		self.CancelButton = self._button_cancel
		self.ClientSize = System.Drawing.Size(307, 264)
		self.Controls.Add(self._button_cancel)
		self.Controls.Add(self._button_ok)
		self.Controls.Add(self._groupBox_place_text)
		self.Controls.Add(self._groupBox_style)
		self.Controls.Add(self._groupBox_coordinates)
		self.Controls.Add(self._groupBox_datum)
		self.Name = "Form1"
		self.Text = "V5 Body Line Scribing"
		self._groupBox_datum.ResumeLayout(False)
		self._groupBox_coordinates.ResumeLayout(False)
		self._groupBox_style.ResumeLayout(False)
		self._groupBox_place_text.ResumeLayout(False)
		self.ResumeLayout(False)

def DoSomething():
    form = Form1()
    if form.ShowDialog() == System.Windows.Forms.DialogResult.OK:
        print "Datum: radioButton top left checked:", form._radio_top_left.Checked
        print "Datum: radioButton top right checked:", form._radio_top_right.Checked
        print "Datum: radioButton bottom left checked:", form._radio_bottom_left.Checked
        print "Datum: radioButton bottom right checked:", form._radio_bottom_right.Checked
        print "Coordinates: radioButton XYZ checked:", form._radioButton_XYZ.Checked
        print "Coordinates: radioButton TBH checked:", form._radioButton_TBH.Checked
        print "Coordinates: radioButton LWH checked:", form._radioButton_LWH.Checked
        print "Style: radioButton millimeters checked:", form._radioButton_mm.Checked
        print "Style: radioButton centimeters checked:", form._radioButton_cm.Checked
        print "Place Text: radioButton Automatic checked:", form._radioButton_automatic.Checked
        print "Place Text: radioButton Manual checked:", form._radioButton_manual.Checked

if __name__=="__main__":
    DoSomething()