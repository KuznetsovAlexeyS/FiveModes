      function fun(k)
      implicit real *8(a-z)
	common /com/Ra,Rae,kluch 
      pi=3.1415
c	926535897932384626433832795


      fun=2*pi**4*(1+k**2)**3*(1-k**2)/k**4-Ra

      return
      end      