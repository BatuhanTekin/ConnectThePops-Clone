using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace _game.Utility.ObjectPooling
{
	public interface IPoolingObject 
	{
    	void OnSpawn();
    	void SetEnable();
    	void SetDisable();
	    bool IsDisabled();
	}
}

