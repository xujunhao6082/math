import atexit
import random
import turtle as tt
###
tt.ht()
tt.tracer(False)
tt.pu()
tt.goto(-1000,0)
tt.pd()
tt.goto(1000,0)
tt.pu()
tt.goto(-400,0)
tt.pd()
###
P = [[1000 for j in range(5)] for i in range(6)]
S = [False,False,False]
SF=0
def R(i):
    global S
    global SF
    if i==0:
        if S[0]:
            return -1
        else:
            if S[1]:
                return -1
            else:
                S[0]=True
                return 2
    elif i==1:
        if S[1]:
            return -4
        else:
            S[1]=True
            if S[0]:
                if S[2]:
                    S=[False,False,False]
                    SF+=1
                    return 10
                else:
                    return 2
            else:
                if S[2]:
                    S=[False,False,False]
                    SF-=1
                    return -10
                else:
                    return -1
    elif i==2:
        S[2]=not S[2]
        if S[0]:
            if S[1]:
                S=[False,False,False]
                SF+=1
                return 10
            else:
                return 0
        else:
            if S[1]:
                S=[False,False,False]
                SF-=1
                return -10
            else:
                return -1
    elif i==3:
        if S[1]:
            S[1]=False
            if S[0]:
                return -4
            else:
                return 0
        else:
            return -1
    elif i==4:
        S[0]=False
        return -1
    return 0
def SN():
    return S[0]+2*S[1]+4*S[2]
@atexit.register
def exit():
    print(P)
    print(SF)
###
ind = 0
SF0=[0 for i in range(50)]
def TP(LIS):
    T=0
    for num in LIS:
        T+=num
    return T/len(LIS)
###
while True:
    SP=0
    i=0
    SNS=SN()
    for j in range(5):
        SP+=P[SNS][j]
    for j in range(5):
        if j>=4:
            break
        if P[SNS][j]/SP>random.random():
            break
        SP-=P[SNS][j]
        i+=1
    D=R(i)
    P[SNS][i]+=D
    ###
    SF0.insert(0, SF)
    SF0.pop()
    tt.setx(-500+ind/50)
    tt.sety((SF-TP(SF0))*10)
    tt.update()
    ind+=1
    ###