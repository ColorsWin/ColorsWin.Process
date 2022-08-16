

1、安装服务
   sc create ProcessServiceDemo binpath= "D:\Demos\ProcessServiceDemo\bin\Debug\ProcessServiceDemo.exe" DisplayName= "ColorsWin  Service"  start= auto

2、卸载服务
  sc delete ProcessServiceDemo

3、启动服务
  net start ProcessServiceDemo

4、停止服务
   net stop ProcessServiceDemo

5、设置启动方式
  sc config ProcessServiceDemo start=AUTO