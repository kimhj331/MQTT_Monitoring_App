   M S B A M L   `   `   `  ����L  HMqttMonitoringApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MqttMonitoringApp.ViewsMqttMonitoringApp.Views  5      )    #MqttMonitoringApp.Views.HistoryView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      R NOxyPlot.Wpf, Version=2.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5      1local%clr-namespace:MqttMonitoringApp.Views  5      .��5
      �   � RowDefinitions  5   
   �� 5   
   $��50 �6   �� 5   
   $��* �6   .+�5      �� $��IoT History Datas��6      MahApps.Styles.Label#���   6(   $��25(�5      $
��Bold"�6   � 5      $��1��6     5   
   �� 5      $��40 �6   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      $	��100 �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Division��6   $��Center=�6'   �� 5      -   5         ��Name$ 	Divisions��5      $	��150��6*    �� DisplayMemberPath$ DivisionVal��66   $��Center=�5        MahApps.Styles.ComboBox#���  5        �� SelectedItem 5!      �� *SelectedDivision+�� 5"      $��    StartDate��6   $	��100��6,   $��Center=�68   (  "System.Windows.Controls.DatePicker  5#      -   5          Name$ 	StartDate��5#      $	��100��6,   $��Center=�68      Text$ 
2020-09-01��6S     MahApps.Styles.DatePicker#���  5$      �� 5%      $��  ~ EndDate��6   #���   6*   $��Center=�6Y     5&      -   5        $ EndDate��5&      $	��100��6*   $ 
2020-09-02��66   $��Center=�5'      #���  5(      �� 5)      $��  Total Count��6   $��Center=�6,   �� 5*      -   5         ��Name$ 
TotalCount��5*      $	��100��6(   $��Center=�64   5+      �� 5-   
   -   5        	 ��Name$	 Search��5-      $��1��6!   $��SEARCH��61   $
��10,0q�6B    ( $MahApps.Styles.Button.Dialogs.Accent#���  5.      � 50   
   $��1��6   $��0��6   $��2��6,   $��10q�6@   .+�51        OxyPlot.Wpf.Plot  
   Series
 52        OxyPlot.Wpf.LineSeries  53        OxyPlot.Wpf.Series   Color$ DarkRed��6%      
MarkerType$
 None=�65      Title$ Temperature��6G   ��54   )   �� *
TempValues+  55      $ 
Aquamarine��6%   $
 None=�68   $ Humidity��6J   ��56   )   �� *HumidValues+  57      $ 
BlueViolet��6%   $
 None=�68   $ Pressure��6J   ��58   )   �� *PressValues+5:      5;      5<      5=      5>      