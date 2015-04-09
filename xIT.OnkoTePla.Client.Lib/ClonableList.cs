using System;
using System.Collections;
using System.Collections.Generic;


namespace xIT.OnkoTePla.Client.Lib
{
	/// <summary>
	/// A <c>ClonableList</c> is a partial wrapped <c>System.Collections.Generic.List</c> extended by a deep- and shallow copy function.	
	/// </summary>
	/// <typeparam name="T">IGenericClonable</typeparam>
	
	public class ClonableList<T> : IClonableList<T> where T : IGenericClonable<T>
	{
		private readonly List<T> _list;

		#region constructors

		public ClonableList()
		{
			_list = new List<T>();
		}

		public ClonableList(int capacity)
		{
			_list = new List<T>(capacity);
		}

		public ClonableList(List<T> initList)
		{
			_list = initList;
		} 

		#endregion

		#region IClonableList<T> members

		private IClonableList<T> CloneList(Func<T, T> copyFunc)
		{
			var resultList = new ClonableList<T>();

			foreach (var genericClonable in _list)
			{
				resultList.Add(copyFunc(genericClonable));
			}

			return resultList;
		} 

		public IClonableList<T> DeepCopy()
		{
			return CloneList(genericClonable => genericClonable.Clone());
		}

		public IClonableList<T> ShallowCopy()
		{
			return CloneList(genericClonable => genericClonable);
		}

		#endregion

		#region IList<T>/IReadonlyList<T> members

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return _list.Remove(item);
		}

		public int Count
		{
			get { return _list.Count; }
		}
		
		public bool IsReadOnly
		{
			get { return (_list as IList<T>).IsReadOnly; }
		}

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return _list[index]; }
			set { _list[index] = value; }
		}

		#endregion

		#region List<T> Wrapper
		
		public void AppendList(List<T> items)
		{
			_list.AddRange(items);
		}

		public void AppendList(IClonableList<T> items)
		{
			_list.AddRange(items);
		}

		public void Sort()
		{
			_list.Sort();
		}
		
		public void Sort(IComparer<T> comparer)
		{
			_list.Sort(comparer);
		}
		
		public void Sort(Comparison<T> comparison)
		{
			_list.Sort(comparison);
		}
		
		#endregion
	}
}
