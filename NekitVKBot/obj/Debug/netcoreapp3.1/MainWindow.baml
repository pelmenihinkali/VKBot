   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   E ANekitVKBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:NekitVKBot
NekitVKBot 5   	      NekitVKBot.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   *localclr-namespace:NekitVKBot     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ AnyBot��5	   	   $	��450��6   $	��800��6%   .��5
      �  � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      $
��431* �6   .+�5   
   o� -   5         o�Name$
 Menu��5      $��30��6   $	��Top=�6)   $��2��6A   ��5      �� 5       �� Color# � ��6"   .@�5      m� $��Настройки��6   .@�5      m� -   5        $2��,Подключение к соц. сетям��5      6=   m� 5      -   5        $/��)Настройка компонентов��5      6:   5      m� 5      -   5         m�Name$ Start��5      $	��►��6&   ���	#CC57B443
�������$��23��6[   $!��Franklin Gothic Medium Cond)�6g   $��32��6�    J� 	AllowDrop$
 True��6�   4  .System.Windows.Automation.AutomationProperties   HelpText$% Запустить проект��6�      
ItemStatus$ status��6  $��18(�61  $
��Bold"�6?  ��6�   Z� 6?  5      �� 5   
   $��0,27,0,0q�6   $��1��6'   .@�5      �� $��Консоль��6   $��22��6'   $��
-2,-2,-2,0q�63   $	��Top=�6G   .��5      � ���	#FFE5E5E5.+�5      7  1System.Windows.Forms.Integration.WindowsFormsHost  $��513,-175,35,468q�6'   	   Child.	 5      %   System.Windows.Forms.WebBrowser  -   5        web��

   Url5   /System.Windows.Forms.WebBrowserUriTypeConverter$*
 $https://jr.samgups.ru/gups/index.php 5   5      ScriptErrorsSuppressed$
 True��6`   5      �� 5      -   5         ��Name$ 	TBConsole��5   "   $��10,10,10,10q�65   $��Normal"�6J   5       5!      5"      5#      5%      5&      