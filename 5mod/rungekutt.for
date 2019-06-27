      subroutine RungeKutt(a,b1,n,y0,eps,y)
      implicit real*8(a-z)
      integer i,ii,j,k,n,flag1,max,i1
      logical l,l1,ll
      dimension y(6),y0(6),y00(6)
      common /blk1/ dt
 
      open(1,file='rez1.dat')
      open(2,file='rez2.dat')
      open(3,file='rez3.dat')
      open(4,file='rez4.dat')
      open(5,file='rez5.dat')
      open(6,file='rez6.dat')
      open(7,file='rez7.dat')         
      max=100000000
      ll=.false.   
      l1=.false.
      x0=a
      x=x0
	dx=dt
      h=dx*0.1
      t=eps*1.0e-4
      do 5 k=1,n
5     y(k)=y0(k)
      write(1,40) y(1),y(3)
      write(2,40) y(2),y(3)
      write(3,40) y(1),y(2)
c      write(4,*) y(1)
      write(4,40) x,y(1)
      write(5,40) x,y(2)
      write(6,40) x,y(3) 
      write(7,40) x,y(4)
      
      do 70 ii=1,max
      if (abs(b1-x).lt.dx) then
      dx=b1-x
      ll=.true.
      end if 
      x1=x+dx  
      l=.false.          
      do 50 i=1,max
      if (abs(x1-x).lt.h) then
      h=x1-x
      l=.true.     
      end if
      do 15 k=1,n
15    y00(k)=y(k)      
      do 20 j=1,10
      call merson(x0,y00,n,h,y,r)
      x=x0+h     
      h=h*(eps/(abs(r)+t))**0.2
      if (h.lt.1.0e-10) then h=1.0e-10
      if (abs(r).lt.eps) go to 30
20    continue
30    continue
      x0=x
      if (l) goto 60
50    continue
60    continue     
      write(1,40) y(1),y(3)  
      write(2,40) y(2),y(3)
      write(3,40) y(1),y(2)
c      write(4,*) y(1)
      write(4,40) x,y(1)
      write(5,40) x,y(2)
      write(6,40) x,y(3)       
      write(7,40) x,y(4)     
      if (ll) go to 80
70    continue
80    continue
40    format(1x, 1e15.8,' ',1e15.8)
45    format(1x, 1e13.6,' ',1e13.6,' ',1e13.6)
46    format(1x, 1e13.6,' ',1e13.6,' ',i8)
      close(1)
      close(2)
      close(3)
      close(4)
      close(5)
      close(6)
      close(7)         
      return
      end 
c...........................................
      subroutine merson(x0,y0,n,h,y,r)
      implicit real*8 (a-z)
      integer i,n
      dimension k1(6),k2(6),k3(6),k4(6),k5(6),x(6),y0(6),y(6),e(6)
      call f(x0,y0,n,h,k1)

      do 10 i=1,n
      x(i)=x0+h/3
      y(i)=y0(i)+k1(i)/3
10    continue
 
      call f(x,y,n,h,k2)

      do 20 i=1,n
      x(i)=x0+h/3
      y(i)=y0(i)+k1(i)/6+k2(i)/6
20    continue
 
      call f(x,y,n,h,k3)

      do 30 i=1,n
      x(i)=x0+h/2
      y(i)=y0(i)+k1(i)/8+k3(i)*3/8
30    continue
  
      call f(x,y,n,h,k4)

      do 40 i=1,n
      x(i)=x0+h
      y(i)=y0(i)+k1(i)/2-k3(i)*3/2+2*k4(i)
40    continue
 
      call f(x,y,n,h,k5)

      r=0.0d0
      t=0.0d0
      do 50 i=1,n
      y(i)=y0(i)+(k1(i)+4*k4(i)+k5(i))/6
      e(i)=(2*k1(i)-9*k3(i)+8*k4(i)-k5(i))/30
      r=r+abs(e(i))
      t=t+abs(y(i))
50    continue
      r=r/(t+1.0d-10)      
      return
      end   
