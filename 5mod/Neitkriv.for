       program neitkriv
c     Изучение электроконвекции слабопроводящих жидкостей
c     в переменном электрическом поле горизонтального 
c     конденсатора с помощью пятимодовой модели.    
      implicit real*8(a-z)
      integer i,j,ii,flag1,flag2,n,kl,l,i1,j1
      dimension y0(6),y(6)
      common /blk/Pr,r,e0,b,d,nu
      common /blk1/dt	

      pi=3.141592653
      open(1,file='param.dat')
      open(11,file='rez11.dat')
      open(12,file='rez12.dat')
      read(1,10) Pr
      read(1,10) nu
      read(1,10) dnu
      read(1,10) nuend
      read(1,10) e0
      read(1,10) de
      read(1,10) eend
      read(1,10) r  
      read(1,10) t0
      read(1,10) tend
      read(1,10) dt      
      read(1,10) k      
      read(1,20) n 
      read(1,30)(y0(i),i=1,5)
      read(1,10) eps     
      read(1,10) ea
      read(1,10) eo 
	read(1,20) flag1
	read(1,20) kl
      read(1,20) flag2
      read(1,10) dx
      read(1,10) dz
      close(1)
	y0(6)=0.d0

	Rac=pi**4*(1+k**2)**3/k**2
	Raec=3*pi**4*(1+k**2)**3/(8*k**2)
	Rae=e0*Raec
	Ra=r*Rac
	print*,'Ra=',Ra,'Rac=',Rac
	print*,'Rae=',Rae,'Raec=',Raec
      b=4/(1+k**2)
      d=(4+k**2)/(1+k**2)
      print *,'k=',k,' r=',r
      print *,'e0=',e0,' nu=',nu
      print *,'b=',b,' d=',d

	if(flag1.eq.1) goto 2	 
 	do 1 ii=1,10000
	print*
	call RungeKutt(t0,tend,n,y0,eps,y)
	Nus=1-2*y(6)/tend
	if(kl.eq.1) then
	write(11,9)e0,Nus 
      print*,'e0=', e0,' Nu=',Nus,i1
      e0=e0+de
	if(e0.ge.eend) goto 2
	endif 
      if(kl.eq.2) then
 	write(11,9)nu,Nus
      print*, 'nu=',nu,' Nu=',Nus
	nu=nu+dnu
      if(nu.eq.nuend) goto 2
      endif
1     continue
2     continue

	if(flag1.eq.1) then
      call RungeKutt(t0,tend,n,y0,eps,y)
	Nus=1-2*y(6)/tend	
      print*, Nus	        
      if(flag2.eq.1)then
 	x=0.
	xend=2/k
	z=0.
	zend=1.0
	a1=sqrt(2.0)*(1+k**2)*y(1)/k
      a2=sqrt(2.0)*(1+k**2)*y(4)/k
	b1=sqrt(2.0)*y(2)/pi
	b2=sqrt(2.0)*y(5)/pi
	c=y(3)/pi
	do 6 i1=1,10000
	do 4 j1=1,10000
	psi=(a1*sin(pi*z)+a2*sin(2*pi*z))*sin(pi*k*x)
c      teta=(b1*sin(pi*z)+b2*sin(2*pi*z))*cos(pi*k*x)+c*sin(2*pi*z)
	teta=1-z+(b1*sin(pi*z)+b2*sin(2*pi*z))*cos(pi*k*x)+c*sin(2*pi*z)
	ro=-pi*(b1*cos(pi*z)+2*b2*cos(2*pi*z))*cos(pi*k*x)-
     1 c*2*pi*cos(2*pi*z)
	write(12,11)x,z,psi,teta,ro
	z=z+dz
	if(z.ge.zend)goto 5
4     continue
5     continue
      x=x+dx
	z=0.
	if(x.ge.xend)goto 7
6     continue
7     continue
      endif
      endif
      close(11)
      close(12)
9     format(1x, 1e13.6,' ',1e13.6)
10    format(1e19.6)
11    format(1x, 1e13.6,' ',1e13.6,' ',1e13.6,' ',1e13.6,' ',1e13.6)	   
20    format(i5)
30    format(5(1e7.2))
      stop
      end      