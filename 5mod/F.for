      subroutine f(t,y,n,h,ks)
      implicit real*8(a-z )
      integer i,n
      dimension ks(n),y(n),fun(6) 
      common /blk/ Pr,r,e0,b,d,nu
      pi=3.141592653
      omega=2*pi*nu
c	print*, Pr,r,e0,b,d,nu
      e=e0*cos(omega*t)**2
	fun(1)=-Pr*y(1)+Pr*(e*y(5)+r*y(2))
	fun(2)=y(1)*y(3)+y(1)-y(2)
	fun(3)=-y(1)*y(2)-b*y(3)
	fun(4)=-Pr*d*y(4)+(Pr/d)*(r*y(5)-e*y(2))
      fun(5)=y(4)-d*y(5)
      fun(6)=y(3)
      do 10 i=1,n
10    ks(i)=h*fun(i)
      return
      end