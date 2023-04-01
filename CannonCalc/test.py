import GunModel
import math
import timeit
model = GunModel.GunModel(1,1,1,1,(math.pi/180*0.001,math.pi/180*0.001))
'''sum = 0
N = 1
rep = 1
for i in range(1,N+1):
    sum += timeit.timeit(f"model.calc({0.4/100*i})",globals=globals(),number=rep)
print('总时:',sum)
sum /= N*rep
print(f'运算次数:{N}*{rep}')
print('平均:',sum)'''
print(math.degrees(model.calc(100)))