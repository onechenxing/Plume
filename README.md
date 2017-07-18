# Plume
New Game Script Language

## 简介：
新的游戏脚本语言。主要用于游戏剧情等脚本编写。
当前版本使用C#实现，可以很好的嵌入Unity游戏引擎。

## 特点：
可同时在主线程运行多个虚拟机，语言自带等待(wait)函数，可方便实现语言级的等待功能。

详细文档请参考Doc目录。

## Demo:
任务：村长的问候

点击说话模块代码(speak_pack.plu)
```
//点击说话函数
Speak = :role,txt{
	role.Speak:txt
	wait:0.1//等待0.1秒,防止连续触发点击
	wait:Unity.Click:
	role.Speak:''//清空说话
}
```

任务代码(npc_chief_hello.plu)
```
task = '村长的问候'

//停止用户控制
player.StopControl:

//获取任务步骤
flag = TaskModel.GetTaskStep:task

//加载说话模块
speak_pack = load:'speak_pack.plu'
Speak = speak_pack.Speak

//获取NPC
chief = NPCManager.GetNPC:'村长'

//记录每个步骤调用的函数
step = [];

// 0：初始的问候
step[0] = {
	wait:chief.move.MoveBy:0,1
	Speak:chief,'欢迎来到新羽村'
	Speak:player,'你是谁?'
	Speak:chief,'我是这里的村长'
	TaskModel.SetTaskStep:task,1	
}

// 1：再次对话，给任务
step[1] = {
Speak:chief,'村子的附近有个大树王'
	Speak:chief,'传说那是远古时代就一直生长的'
	Speak:chief,'它保护着村子和村民'
	Speak:chief,'你去看看吗?'
	wait:UI.SelectList:['好','不']
	if UI.select == '好'{
		Speak:chief,'你出了村子往西方走就看到了'
	}
	if UI.select == '不'{
		Speak:chief,'年轻人要多走动走动才好'
	}
	TaskModel.SetTaskStep:task,2
}

// 2：再次对话，...
step[2] = {
	Speak:chief,'...'
}

//根据任务进度调用具体步骤
step[flag]:


//开启用户控制
player.StartControl:

```
