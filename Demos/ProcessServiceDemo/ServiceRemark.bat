

1����װ����
   sc create ProcessServiceDemo binpath= "D:\Demos\ProcessServiceDemo\bin\Debug\ProcessServiceDemo.exe" DisplayName= "ColorsWin  Service"  start= auto

2��ж�ط���
  sc delete ProcessServiceDemo

3����������
  net start ProcessServiceDemo

4��ֹͣ����
   net stop ProcessServiceDemo

5������������ʽ
  sc config ProcessServiceDemo start=AUTO