import math
import time
import turtle
x_n=[0,1,0,0,0]
x_o=x_n*1
dx=0.00002
t=0
μ=0.1
g=9.8
L=1
px=(1000,800)
ls=[50,50]
turtle.screensize(px[0],px[1], "white")
turtle.register_shape("1", ((1, 1), (-1, 1), (-1, -1),(1,-1)))
turtle.setup(width=math.floor(px[0])+50,height=math.floor(px[1])+50)
turtle.shape('1')
turtle.speed(0)
turtle.delay(0)
turtle.penup()
turtle.goto(0,0)
turtle.pendown()
turtle.pencolor("#FF0000")
turtle.goto(-turtle.screensize()[0],0)
turtle.goto(turtle.screensize()[0],0)
turtle.goto(0,0)
turtle.goto(0,-turtle.screensize()[1])
turtle.goto(0,turtle.screensize()[1])
turtle.goto(0,0)
turtle.penup()
turtle.pencolor("#000000")
turtle.goto(ls[0]*x_n[0],ls[1]*x_n[1])
turtle.pendown()
while True:
    if math.floor(t%((1/dx)/1000)*1000)<=0:
        print(x_n[0],"\t\t",x_n[1],"\t\t",math.floor(t*dx*100000)/100000)
        turtle.goto(ls[0]*x_n[0],ls[1]*x_n[1])
    #单摆,μ:阻力项,L:摆长
    #x_n[1]+=((-μ*x_o[1]-(g/L)*math.sin(x_o[0])))*dx
    #x_n[0]+=x_o[1]*dx
    #爱情,μ:阻力项
    #x_n[0]+=(-μ*x_o[0]-x_o[1])*dx
    #x_n[1]+=x_o[0]*dx
    #differential equation code start
    #some code...
    #differential equation code end
    x_o=x_n*1
    t+=1
