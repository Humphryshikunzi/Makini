import platform 

s = "Files\\Images/Cars\\Humphry_Car_2 (16).PNG"
t= 0
system = platform.system() 

if(system=='Linux'):    
    t = s.rfind('/')
elif(system=='Windows'):
    t= s.rfind('\\')
file_name = s[t+1:] 