import wpf
from System.Windows import Application, Window
class myWPF(Window):
    def __init__(self):
        component = wpf.LoadComponent(self,'WpfApplication2.xaml')        
        component.textBox.Text = "Hello"

if __name__ == '__main__':
    # print(dir(myWPF()))
    Application().Run(myWPF())
    