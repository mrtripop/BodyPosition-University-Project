   M S B A M L   `   `   `  ����G  CBodyPosition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:BodyPosition.MVVM.ViewBodyPosition.MVVM.View  5      )    #BodyPosition.MVVM.View.RegisterPage   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:BodyPosition.MVVM.View     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � Title$  RegisterPage��5
      .��5      � $	��780��6   $	��370��5      .+�5   
   �� 
�������$	��Top=�5       �� Padding$ 20q�5       �� CornerRadius$ 10��5       J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 30Q�6#      Color$ 	LightGray��5   #      ShadowDepth$ 0Q�5   #   ._�5      �� .+�5      �� $��User Information��6   $��30(�5      $
��Bold"�6*   $��	30,0,0,20q�5      �� 5       �� Orientation$ 
Horizontal=�6   $��0,15,0,0q�66   $��25��6H   $��Center=�5      .+�5      �� $��
First Name��6    $��Center=�62   $	��100��5        �� 5"      -   5         ��Name$ 	firstName��5"      $	��175��5#      �� 5%      $��	Last Name��6    $��Center=�61   $	��100��5&       $��50,0,0,0q�5'       �� 5)      -   5        $ lastName��5)      $	��175��5*      5+      �� 5-      $ 
Horizontal=�6   $��0,15,0,0q�66   $��25��6H   $��Center=�5.      .+�5/      �� $��Gender��6    $��Center=�6.   $	��100��50       �� 52      -   5        	 ��Name$	 gender��52      )  #MaterialDesignThemes.Wpf.HintAssist
   Hint$
 Gender��53      $	��175��54      .@�55      �� $
��Male��6'   �� 56      $��Female��6'   57      �� 59      $��Phone��6    $��Center=�6-   $	��100��5:       $��50,0,0,0q�5;       �� 5<      -   5        $ phone��5<      $	��175��5=      5>      �� 5@      $ 
Horizontal=�6   $��0,15,0,0q�66   $��25��6H   $��Center=�5A      .+�5B      �� $��Weight��6    $��Center=�6.   $	��100��5C       �� 5D      -   5        $ weight��5D      $	��175��5E      �� 5G      $��Height��6    $��Center=�6.   $	��100��5H       $��50,0,0,0q�5I       �� 5J      -   5        $ height��5J      $	��175��5K      5L      �� 5N      $ 
Horizontal=�6   $��0,15,0,0q�66   $��25��6H   $��Center=�5O      $	��600��6:   .+�5P      �� $
��Date��6    $��Center=�6,   $	��100��5Q       (  "System.Windows.Controls.DatePicker  5S      -   5          Name$
 date��5S   !   $	��175��6/   6l   5T      �� 5V      $ 
Horizontal=�6   $��0,15,0,0q�66   $��25��6H   $��Center=�5W      �� 5[      $ 
Horizontal=�6   $��0,15,0,0q�66   $��Center=�5\      .+�5^      �� -   5        $	��150��5^      $��50��6)   $
��SAVE��5_      
����������#07223F���#07223F$��	10,0,10,0q�5c      �� 5e      -	   5        $	��150��5e      $��50��6)   $��CANCEL��5f      ���#07223F
�������
�������$��	10,0,10,0q�5j      5l      5m      5n      5o      5p      