   M S B A M L   `   `   `  ����@  <Teszt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:VersenyVerseny  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	       Verseny.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Verseny     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   R NOxyPlot.Wpf, Version=2.2.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0Y UOxyPlot.Wpf.Shared, Version=2.2.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0"oxyhttp://oxyplot.org/wpf   5   	     � Title$  
MainWindow��5
   	   $
��auto��6   $
��auto��6*   .��5      � .+�5   
   �� .@�5      �� $��	Dashboard��6   .��5      � $��20q�6    � ColumnDefinitions 5      �� 5      $��1* �6+   �� 5      $��1* �6+    � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $
��Auto �6(   .+�5      �� $��0��6   $��0��6*   $��2��6:    �� Padding$ 10q�6N   
������� �� CornerRadius$ 10��6r   ._�5      �  5      �� 5   "   $
��Auto �60   �� 5   "   $
��Auto �60    5!      �� 5"   "   $��1* �63   �� 5#   "   $��1* �63   .+�5&      �� $��0��6)   $��0��66   $��Center=�6F   .+�5'   "   �� -   5         ��Name$ totalCustomers_lb��5'   (   $��Total Customers: 2��6C   �� 5(   "   -   5        $ averageAge_lb��5(   (   $��Average Age: 2��6?   �� 5)   "   -   5        $% mostFrequentPurchaseCategory_lb��5)   (   $,��&Most Frequent Purchase Category: bread��6Q   �� 5*   "   -   5        $ totalPurchaseValue_lb��5*   (   $��Total Purchase Value: 4��6G   �� 5+   "   -   5        $ averageOrderValue_lb��5+   (   $��Average Order Value: 5��6F   5,      &   System.Windows.Controls.DataGrid  5.      -   5          Name$ frequencyDataGrid��5.   '   $��0��5/      $��1��6(      AutoGenerateColumns .     CanUserAddRows .  	   HeadersVisibility$	 Column=�52      
   
IsReadOnly
 . $	��100��54      $��10q�6(      Columns 55   "   0  *System.Windows.Controls.DataGridTextColumn  56   &   ,  &System.Windows.Controls.DataGridColumn   Header$
 Year��69      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6`   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6G   �� *Year+6`     57   &   $ Total Frequency��69   $ * 6u    6R   �� *TotalFrequency+6u   59      5:      5;      � 5>      $��1��6   $��0��6(   $��10q�68   .+�5?        LiveCharts.Wpf.PieChart  -   5          Name$ Nemek��5?   '   &   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Bottom=�66      Series 5A   "     LiveCharts.Wpf.PieSeries  	  LiveCharts.Wpf.Series 	  Title$ Kategória 1��60    	  Values;
  5LiveCharts.Wpf.Converters.NumericChartValuesConverter$ 30
 6D    	  
DataLabels . 6P     5B   "   $ Kategória 2��60   $ 20
 6D    . 6P     5C   "   $ Kategória 3��60   $ 50
 6D    . 6P   5E      5F      � 5H      $��1��6   $��1��6(   $��10q�68   .+�5I        -   5        $ Tagsag��5I   '   $ Bottom=�67    5K   "     $ Kategória 1��60   $ 30
 6D    . 6P     5L   "   $ Kategória 2��60   $ 20
 6D    . 6P     5M   "   $ Kategória 3��60   $ 50
 6D    . 6P   5O      5P      � 5S      $��2��6   $��0��6(   $��10q�68   .+�5T      #  LiveCharts.Wpf.CartesianChart  -	   5          Name$ MostPurchasedCategory��5T   -   $
 Left=�5U   !   $��Stretch=�5V   !   $��Stretch=�5W   !      AxisX 5Y   "     LiveCharts.Wpf.Axis     Title$ Category��6+      Labels 6<   �� *Labels+   AxisY 5\   "        LabelFormatter 6+   �� *	Formatter+5^      5_        5b      -
   5        $ Top10DataGrid��5b      $��2��5c      $��1��6     .   .  $	 Column=�5f      
 . $��10q�5h      ��. $
��Auto=�5j      $
��Auto=�5k       5l        5m      $
 Rank��61   $
 auto 6X    6?   �� *Rank+6X     5n      $
 Name��61   $
 auto 6X    6?   �� *Name+6X     5o      $ Spending��61   $
 auto 6`    6C   �� *
Spending+6`   5q      � 5t      $��3��6   $��3��6(   $��
20,20,20,0q�6<   .+�5u        -   5        $ YearlyTrendChart��5u   -   $
��auto��5v   -   $	��100��5w   -   $
 Left=�5x   -   $��Stretch=�5y   -   $��Stretch=�5z   -    5|   "     $ Years��6+    69   �� *MonthLabels+ 5   "     $ 	Customers��6+    6=   �� *	Formatter+5�      5�      5�      5�      �� 5�      $��Customers list��6   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $	��19* �6+   �� 5�      $
��775* �6+   .+�5�        -   5        $ customers_datagrid��5�   /   $��1��6   $��0,0,0,93q�6K   �� 5�      $��1��6   $��Search��6,   $
��Left=�6=   $��	20,0,0,20q�6X   $��Bottom=�6k   �� 5�      -   5         ��Name$ 
kereses_tb��5�   .   $��1��6   $
��Left=�6B   $��	80,0,0,20q�6]    �� TextWrapping$
 Wrap=�6p   $�� ��6�   $��Bottom=�6�   $	��500��6�   $��26��6�   �� 5�      -   5         ��Name$ Search��5�   -   $��1��6   $��Search��6=   $
��Left=�6N   $��
623,0,0,20q�6i   $��Bottom=�6}   $��28��6�   $��80��6�   6�   �� 5�      -   5        $ pageNumber_lb��5�      $��1��63   $��1-25 customers��6C   $
��Left=�6\   $��
310,0,0,60q�6w   $��Bottom=�6�   �� 5�      -   5        $ pagePlusz_btn��5�      $��1��64   $��Right��6D   $
��Left=�6T   $��
430,0,0,60q�6o   $��Bottom=�6�   $��36��6�   6�   �� 5�      -   5        $ pageMinus_btn��5�      $��1��64   $
��Left��6D   $
��Left=�6S   $��
250,0,0,60q�6n   $��Bottom=�6�   $��36��6�   6�   5�      5�      �� 5�      $��File operating��6   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $	��19* �6+   �� 5�      $
��775* �6+   .+�5�      �� $��1��6#   $
��Auto=�63   $
��Auto=�6V   .��5�      �� $��	44,33,0,0q�6%   .+�5�      �� -   5        $��	Open file��5�   %   $
��Left=�69   $	��Top=�6T   $��64��6�   $	��310��6�   �� 5�      -   5        $ DatasDownload_btn��5�   %   $��Download corrected data��6@   $
��Left=�6b   $	��Top=�6}   $��61��6�   $	��310��6�   $��0,20,0,0q�6�   �� 5�      -   5        $ ErrorsDownload_btn��5�   %   $��Download errors��6A   $
��Left=�6[   $	��Top=�6v   $��61��6�   $	��310��6�   $��0,20,0,0q�6�   �� 5�      -   5        $ DatabaseAdd_btn��5�   %   $2��,Appending data from the file to the database��6>   $
��Left=�6u   $	��Top=�6�   $	��310��6�   $��61��6�   $��0,20,0,0q�6�   �� 5�      -   5        $ progress_lb��5�   $   $��Progress: [0/710]��69   6W   5�      5�      5�      5�      �� 5�      -   5         ��Name$ LoadedFile_tabitem��5�   ,   $��Loaded items��6   .��5�      � ���	#FFE5E5E5 J� RenderTransformOrigin$ 0.5,0.5&�6.   z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�        �� Angle$  0.021Q�6.   T� 5�      5�       5�      �� 5�      $	��19* �6+   �� 5�      $
��775* �6+   .+�5�        -   5        $ loaded_datagrid��5�   /   $��1��6   $��0,0,0,93q�6H   �� 5�      -   5        $ LoadedpageNumber_lb��5�      $��1��69   $��1-25 customers��6I   $
��Left=�6b   $��
100,0,0,60q�6}   $��Bottom=�6�   �� 5�      -   5        $ LoadedpagePlusz_btn��5�      $��1��6:   $��Right��6J   $
��Left=�6Z   $��
260,0,0,60q�6u   $��Bottom=�6�   $��36��6�   6�   �� 5�      -   5        $ LoadedpageMinus_btn��5�      $��1��6:   $
��Left��6J   $
��Left=�6Y   $��	20,0,0,60q�6t   $��Bottom=�6�   $��36��6�   6�   5�      5�      �� 5�      -   5        $ Errors_tabitem��5�      $��Errors list��6.   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $	��19* �6+   �� 5�      $
��775* �6+   .+�5�        -   5        $ errors_datagrid��5�   /   $��1��6   $��	0,0,0,100q�6H   $	��Top=�6[   �� 5�      -   5        $ ErrorpageNumber_lb��5�      $��1��68   $��1-25 customers��6H   $
��Left=�6a   $��
100,0,0,60q�6|   $��Bottom=�6�   �� 5�      -   5        $ ErrorpagePlusz_btn��5�      $��1��69   $��Right��6I   $
��Left=�6Y   $��
260,0,0,60q�6t   $��Bottom=�6�   $��36��6�   6�   �� 5�      -    5        $ ErrorpageMinus_btn��5�      $��1��69   $
��Left��6I   $
��Left=�6X   $��	20,0,0,60q�6s   $��Bottom=�6�   $��36��6�   6�   5�      5�      5�      5�      5�      