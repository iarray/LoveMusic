   M S B A M L   `   `   `  ����   	LoveMusic    LoveMusic.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  
MainWindow��$	��400��$	��590�� � AllowsTransparency .  � WindowStyle$
 None=� � WindowStartupLocation$ CenterScreen=���Z� .���  � ColumnDefinitions �� $	��310 ��� .+�� $��1��$
��Left=�$	��280��.+��� -   $	��tb1��$y�	Collapsed=�$��Images/Lrctext.png��$	��Top=�$
��Left=�$��	10,10,0,0q�$	��260���� -   $	��lr1��$y�	Collapsed=�$��Images/Lrclistview.png��$	��300��$	��260��$��10,41,10,50q�$��1���� -    ��Name$ txtb1��$y�	Collapsed=�$
��Left=�$��40��$��	16,17,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��250�� �� IsUndoEnabled .  {�.  $��14(���Z� ��Z� �� -    ��Name$
 lrc1��$y�	Collapsed=�$
��Left=�$	��170��$��
16,115,0,0q�$	��Top=�$	��250����Z� ��Z� � $
��Left=�$��0��$	��310��.+��� 	 �� CornerRadius$	 5�����	#FF226C8A$��0q�$	��400��$	��310��
 J� Effect
 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 0Q�   Color$ 	#FF7A7F80��._��� $	 5�����	#FF56C5F3$��0q��  � RowDefinitions �� $	��115 ��� .+�� $��0��$	��290��$	��110��.+��� $��Images/控制台2.png��$	��280��$	��110��$��Center=�$��Center=�$��2���� -   $��btClose��$��20��$��20��   ButtonStyleClose#���   $��Right=�$	��Top=�$��	0,12,13,0q��� -   $��btBack��$��1��$��35��$��35��  ButtonStyleBack#���  $	��Top=�$
��Left=�$��15,7,0,8q��� -   $��btPlay��$��1��$��38��$��38��  ButtonStylePlay#���  $	��Top=�$
��Left=�$��53,5,0,8q��� -	   $��btNext��$��1��$��35��$��35��  ButtonStyleNext#���  $	��Top=�$
��Left=�$��95,7,0,8q��� $��Images/矩形进入.png��$��1��$��35��$��62��$��Center=�$��Center=�$��142,10,86,10q��� -
   $��	txtmaohao��$��1��$��:��$��14(�$��20��$��5��$��Center=�$��Center=�$��50,0,0,0q����	#FF837E7E�� -   $��txtMin��$��14(�$��20��$��25��$��1��$��Center=�$��Center=�$��150,17,115,18q����	#FF837E7E���� *	Minutes+�� -   $��txtSec��$��14(�$��20��$��25��$��1��$��Center=�$��Center=�$��90,0,0,0q����	#FF837E7E���� *	Seconds+�� -   $��yinliang��$��1��  sliderstyle#���  $��15��$��50��$��Right=�$��Bottom=�$��	0,0,13,19q���Z� ��Z� �� $��Images/538130.png��$��1��$��26��$��26��$��Center=�$��Right=�$��0,0,65,0q��� -   $��sdMusic��$��15��$	��280��  Slider_CustomStyle#���   �� IsMoveToPointEnabled .  � Cursor$
 Hand�� � Maximum$ 100.00Q�$��6,39,4,1q� � Value �� *	AllTime+
 -  'System.Windows.Media.Effects.BlurEffect     Radius$ 1Q� �� �� � $��1��$	��270��.+��� $'��!Images/透明的下拉菜单1.png��$	��300��$	��270��$��2���� -   $��	musicList��$	��170��$	��260��$��Center=�$��Center=�{�.  J� 	AllowDrop . $��1�����	#FF110101��Z� ��Z� �� -   $��btOpen��$��0��$��35��$��35��  ButtonStyleOpen#���  $��Center=�$
��Left=�$��	30,0,0,10q��� -   $��btLrc��$��0��$��18��$��30��  ButtonStyleLrcClose#���  $��Center=�$
��Left=�$��
200,0,0,10q� �� $��60 ��� 