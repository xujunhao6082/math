from __future__ import annotations
import math


class GunModel:
    __constCache: tuple[float, float, float, float]
    __constCacheWithT: tuple[float, float, float]
    __consts: tuple[float, float, float, float]
    __threshold: tuple[float, float]

    def __init__(self, k: float, m: float, v_0: float, g: float, threshold: tuple[float, float]) -> None:
        '''
        初始化计算器
        :param k: 空气阻力系数(kg*s)
        :param m: 炮弹质量(kg)
        :param v_0: 初速(m/s)
        :param g: 重力加速度(m/s^2)
        :param threshold:停止计算阈值(最小终角变及最小角变加速度)
        '''
        self.setParams(k, m, v_0, g)
        self.setThreshold(threshold)

    def setParams(self, k: float, m: float, v_0: float, g: float):
        '''
        重设模型参数
        :param k: 空气阻力系数(kg*s)
        :param m: 炮弹质量(kg)
        :param v_0: 初速(m/s)
        :param g: 重力加速度(m/s^2)
        '''
        self.__consts = (k, m, v_0, g)
        self.__calcCache()
        self.__constCacheWithT = (1, 1, 1, 1, 1)

    def setThreshold(self, threshold):
        '''
        设置阈值
        :param threshold:停止计算阈值(最小终角变及最小角变加速度)
        '''
        self.__threshold = threshold

    def __calcCache(self) -> None:
        kPm = self.__consts[0]/self.__consts[1]
        kPmv = kPm/self.__consts[2]
        kPmg = kPm/self.__consts[3]
        self.__constCache = (kPm, kPmv, kPmg, 1/self.__consts[2])

    def __equs(self, n):
        if n == 0:
            # 高仰角迭代方程
            return lambda x: self.__constCacheWithT[1]/(1-math.exp(-self.__constCacheWithT[0]/x*(self.__constCache[2]*(1-x**2)**0.5+self.__constCache[3]+self.__constCacheWithT[2]*x)))
        elif n == 1:
            # 低仰角迭代方程
            return lambda x: (1-((self.__constCache[3]+x*math.log(1-self.__constCacheWithT[1]/x)/self.__constCacheWithT[0]+self.__constCacheWithT[2]*x)/self.__constCache[2])**2)**0.5

    def __stopCon(self, ls):
        if isinstance(ls[0], complex) or ls[0] > 1 or ls[0] < 0:
            return -1
        delta = (math.acos(ls[0])-math.acos(ls[1]),
                 math.acos(ls[1])-math.acos(ls[2]))
        if abs(delta[0]-delta[1]) < self.__threshold[1] and abs(delta[0]) < self.__threshold[0]:
            return 1
        return 0

    def calc(self, target, target_h=0):
        if target <= 0:
            raise ValueError('target必须大于0')
        self.__constCacheWithT = (
            self.__constCache[0]*target, self.__constCache[1]*target, target_h/target/self.__consts[3])
        LitX0 = (self.__constCacheWithT[1], 1)
        result = [None, None]
        for n in range(2):
            ls = [LitX0[n] for i in range(3)]
            flag = 0
            equ = self.__equs(n)
            N = 10
            while not flag:
                if n == 1:
                    print(ls[0])
                try:
                    ls.insert(0, equ(ls[0]))
                except ValueError:
                    flag = -1
                except TypeError:
                    flag = -1
                ls.pop(-1)
                if N < 0:
                    flag = self.__stopCon(ls)
                else:
                    N -= 1
            if flag > 0:
                result[n] = math.acos(ls[0])
        return tuple(result)
