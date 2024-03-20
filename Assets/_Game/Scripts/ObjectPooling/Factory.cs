using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace _game.Utility.ObjectPooling
{
	public class Factory<T> : IFactory<T> where T : new() {
		public T Spawn() {
			return new T();
		}
	}
}

