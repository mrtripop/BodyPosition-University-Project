   M S B A M L   `   `   `  ����G  CBodyPosition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:BodyPosition.MVVM.ViewBodyPosition.MVVM.View  5      N JLightBuzz.Vitruvius, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullhGclr-namespace:LightBuzz.Vitruvius.Controls;assembly=LightBuzz.VitruviusLightBuzz.Vitruvius.Controls 5      %    BodyPosition.MVVM.View.HomeView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:BodyPosition.MVVM.View     5      \controlsGclr-namespace:LightBuzz.Vitruvius.Controls;assembly=LightBuzz.Vitruvius     5      .��5      �   � RowDefinitions  5   
   �� 5      $��90 �6   �� 5      .+�5   
   � $��0��6   .+�5      �� -   5         ��Name$ 
backButton��5      $
��Back��5      $
��Left=�5      $��Center=�5      $	��100��5      $��70��5      $��20,0,0,0q�5      $��24(�5      �� 5      $��Body Position��6   $��Center=�5      $��Center=�5      $��64(�5       $��TH SarabunPSK)�5!      
�������$
��Bold"�5#      $��0��5$      5&      �� 5(   
   $��1��6   .+�5*      �  � ColumnDefinitions 5+      �� 5,      $��3* �6'   �� 5-      $��* �6'     50      �� 51      $
��Auto �6$   .+�54      �� $��0��6   $��0��55      ._�56      �  57      �� 58      $��* �6/     5;      �� 5<      $
��Auto �6,   �� 5=      $��* �6,   .+�5@      /  )LightBuzz.Vitruvius.Controls.KinectViewer  -   5          Name$ viewer��5@   0   $��2��6@   (� 5A      .��5B      �� $
��1280��6%   $	��700��62   .+�5C   "   .  (LightBuzz.Vitruvius.Controls.KinectAngle  -   5          Name$ anglePelvisFront��5C   7    J� Opacity$	 0.5Q�6Q      Fill
 � � �6_     5D   "   -   5        $ spaceShoulderRight��5D   7   $	 0.5Q�6S   
 ��  �6a     5E   "   -   5        $ spaceShoulderLeft��5E   7   $	 0.5Q�6R   
 �  ��6`     5F   "   -   5        $ angleSidePelvis��5F   7   $	 0.5Q�6P   
 �����6^     5G   "   -   5        $ 	angleKnee��5G   7   $	 0.5Q�6J   
 � ���6X     5H   "   -   5        $ 
angleAnkle��5H   7   $	 0.5Q�6K   
 ����6Y     5I   "   -	   5        $ dRight��5I   7   $	 0.5Q�6G   
 �   �6U     5J   "   -
   5        $ dLeft��5J   7   $	 0.5Q�6F   
 �   �6T   5K      5L      �� 5O      $��1��6%   $��50��5P   %    �� Orientation$ 
Horizontal=�5Q   %   $��Center=�5R   %   $��Center=�5S   %   .+�5U      �� 
�������$��20��5V   (   $��16(�5W   (   UID:6<   �� 5Y      -   5         ��Name$ userUID��5[   (   
���   �
�������$	��100��5\   (   $��30��5]   (   $��Center=�5^   (   $��16(�5_   (   $��5,0,0,0q�5`   (   6;   �� 5b      
�������$��30��5c   (   $��20,0,0,0q�5d   (   $��16(�5e   (   NAME:6=   �� 5g      -   5        $ userName��5i   (   
���   �
�������$	��300��5j   (   $��30��5k   (   $��5,0,0,0q�5l   (   $��Center=�5m   (   $��16(�5n   (   68   �� 5p      
�������$��20��5q   (   $��20,0,0,0q�64   $��16(�5r   (   TEST:6=   �� 5t      -   5        $ testName��5v   (   
���   �
�������$	��300��5w   (   $��30��5x   (   $��5,0,0,0q�5y   (   $��Center=�5z   (   $��16(�5{   (   68   5|      5~      5      �� 5�      $ 
Horizontal=�6   $��15,0,0,0q�5�      $��0��5�      $
��auto��5�      $��Center=�5�      $��1��5�      .+�5�      �� .+�5�      �� $0��*ค่าพารามิเตอร์��6$   
�������$��24(�5�      $��Center=�5�      $��	20,0,0,20q�5�      � 5�       5�      �� 5�   "   $
��Auto �63   �� 5�   "   $��10 �63   �� 5�   "   $
��Auto �63     5�      �� 5�   "   $
��Auto �60   �� 5�   "   $��* �60   .+�5�      � $��0��6#    5�   "   �� 5�   &   $��3* �67   �� 5�   &   $��* �67   .+�5�   "   �� $��0��6-   $ Vertical=�5�   -   $
��auto��5�   -   	 � 	Resources	 5�   &   
 �� 
TargetType �� Property �� Value��%[   '��       �� 5�   *   "
 ��60   .�5�   .   ��  � ��$ 30,13,30,13q�6G   5�   +   5�   '   .+�5�   &   �� $��Pelvis Front��60   
�������$��18(�5�   $   $
��Left=�5�   $   �� 5�   &   $��Shoulder Right��60   
�������$��18(�5�   $   $
��Left=�5�   $   �� 5�   &   $��Shoulder Left��60   
�������$��18(�5�   $   $
��Left=�5�   $   �� 5�   &   $��Pelvis Side Angle��60   
�������$��18(�5�   $   $
��Left=�5�   $   �� 5�   &   $��
Knee Angle��60   
�������$��18(�5�   $   $
��Left=�5�   $   �� 5�   &   $��Ankle Angle��60   
�������$��18(�5�   $   $
��Left=�5�   $   5�   #   �� 5�   "   $��2��6-   $ Vertical=�5�   -   $��Center=�5�   -   $��0,0,0,0q�5�   -   	 5�   &   ��%�   '��       �� 5�   *   "
 ��60   .�5�   .   ��  � ��$ 0,14,0,0q�6G   �� 5�   .    � ��$ 38��6G   �� 5�   .    � ��$	 200��6F   5�   +   5�   '   .+�5�   &   �� $	��0,0q�6-   $	��100��5�   -   ��5�   *   �� 5�   .    �� Color$ White��6>    �� Opacity$	 0.7Q�6L   ._�5�   *   ��  �� TextAlignment$ Center=�64   $��Center=�5�   ,   $��Center=�5�   ,   �� 6H   -   5         ��Name$ tblPelvisFront��5�   L   $� ��6d   �� 6o   $�°��6s   6   5�   '   �� 5�   &   $	��100��6-   ��5�   *   �� 5�   .   $ White��6>   $	 0.7Q�6L   ._�5�   *   �� $ Center=�64   $��Center=�6K   $��Center=�6h   �� 6�   -   5        $ tblShoulderRight��5�   �   $� ��6�   �� 6�   $	� cm��6�   6�   5�   '   �� 5�   &   $	��100��6-   ��5�   *   �� 5�   .   $ White��6>   $	 0.7Q�6L   ._�5�   *   �� $ Center=�64   $��Center=�6K   $��Center=�6h   �� 6�   -   5        $ tblShoulderLeft��5�   �   $� ��6�   �� 6�   $	� cm��6�   6�   5�   '   �� 5�   &   $	��100��6-   ��5�   *   �� 5�   .   $ White��6>   $	 0.7Q�6L   ._�5�   *   �� $ Center=�64   $��Center=�6K   $��Center=�6h   �� 6�   -   5        $ tblPelvisSideAngle��5�   �   $� ��6�   �� 6�   $�°��6�   6�   5�   '   �� 5�   &   $	��100��6-   ��5�   *   �� 5�   .   $ White��6>   $	 0.7Q�6L   ._�5�   *   �� $ Center=�64   $��Center=�6K   $��Center=�6h   �� 6�   -   5        $ tblKneeAngle��5�   �   $� ��6�   �� 6�   $�°��6�   6�   5�   '   �� 5�   &   $	��100��6-   ��5�   *   �� 5�   .   $ White��6>   $	 0.7Q�6L   ._�5  *   �� $ Center=�64   $��Center=�6K   $��Center=�6h   �� 6�   -   5        $ tblAnkleAngle��5  �   $� ��6�   �� 6�   $�°��6�   6�   5  '   5  #   5     5     �� 5     $��	0,20,0,10q�6$   
�������$��Center=�5	  $   $��24(�5
  $   �� 63   $<�6ปุ่มควบคุมการทำงาน��67   5     � 5     $
��auto��6     5     �� 5  "   $��2* �60   �� 5  "   $��* �60   �� 5  "   $��* �60   .+�5     �� -   5        $ recordButton��5  %   $	��220��5  %   $��60��61   $��Record��5  %   $��0��5  %   
�������$��18(�5  %   �� 5     -   5        $ DatabaseButton��5  %   $	��220��5  %   $��60��61   $��Database��5  %   
�������$��18(�5!  %   $��1��5"  %   $��5q�5#  %   5$     5%     5&     5'     5(     5)     5*     