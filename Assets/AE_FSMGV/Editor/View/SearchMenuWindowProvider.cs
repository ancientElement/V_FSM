using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AE_FSMGV
{
    public class SearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));           //添加了一个一级菜单

            entries.Add(new SearchTreeGroupEntry(new GUIContent("Example")) { level = 1 }); //添加了一个二级菜单
            entries.Add(new SearchTreeEntry(new GUIContent("float")) { level = 2, userData = typeof(StateNodeView) });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("Example2")) { level = 1 }); //添加了一个二级菜单
            return entries;
        }

        public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry,            //声明一个delegate类
               SearchWindowContext context);

        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler;                              //delegate回调方法

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
            {
                return false;
            }
            return OnSelectEntryHandler(searchTreeEntry, context);
        }
    }
}
