// using PDS.Collections;
// using PDS.UndoRedo;
//
// namespace PDS.Implementation.UndoRedo
// {
//     public class UndoRedoDictionary<TKey, TValue> : IUndoRedoDictionary<TKey, TValue>
//     {
//         private readonly IPersistentDictionary<TKey, TValue> _persistentDictionary;
//         private readonly IPersistentStack<IPersistentDictionary<TKey, TValue>> _undoStack;
//         private readonly IPersistentStack<IPersistentDictionary<TKey, TValue>> _redoStack;
//
//         public UndoRedoDictionary()
//         {
//             _persistentDictionary = PersistentDictionary<>.Empty;
//             _undoStack = undoStack;
//             _redoStack = redoStack;
//         }
//     }
// }