      subroutine sekush(x,ea,eo,t,res)
      implicit real*8 (a-z)
      integer i,max,t
      max=100000
      x0=x
      x1=x0*1.1d0
      if(x0.EQ.0.0d0) x1=x0+0.1d0
      do 10 i=1,1000
      if(abs(fun(x1)-fun(x0)).lt.1e-6) then
      x1=x1*1.1d0
      else
      go to 11
      end if			   
10    continue
11    continue    
     
      do 20 i=1,max
      f0=fun(x0)
      f1=fun(x1)
      x2=x1-f1*(x1-x0)/(f1-f0)	
      if (abs(x2-x1).lt.ea+eo*abs(x2)) then 
      go to 30 
      else
      if (i.eq.max) then 
      go to 40
      end if
      x0=x1
      x1=x2
      end if	     
20    continue
30    continue 
c     йнпемэ мюидем
      t=0
      res=x2
      go to 50
40    print *,'root not' 
c     йнпемэ ме мюидем       
      t=1
50    continue   
      return
      end   