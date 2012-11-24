using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace LordJZ.ObjectManagement
{
    public abstract class SingletonAsyncObjectAccessor<T, TKey, TObject>
        : Singleton<T>, IAsyncObjectAccessor<TKey, TObject>
        where T : SingletonAsyncObjectAccessor<T, TKey, TObject>, new()
        where TObject : class
    {
        #region Async

        public virtual async Task<TObject> AsyncCreate()
        {
            return await Task<TObject>.Factory.StartNew(this.Create);
        }

        public virtual async Task AsyncDelete(TKey key)
        {
            await Task.Factory.StartNew(() => this.Delete(key));
        }

        public virtual async Task<TObject> AsyncLoad(TKey key)
        {
            return await Task<TObject>.Factory.StartNew(() => this.Load(key));
        }

        /// <summary>
        /// Asynchronously persists the object using the specified key.
        /// </summary>
        /// <param name="key">
        /// Key to be used when persisting the object.
        /// </param>
        /// <param name="obj">
        /// Object to be persisted.
        /// </param>
        /// <exception cref="CannotSaveObjectException">
        /// The object cannot be saved.
        /// </exception>
        public virtual async Task AsyncSave(TKey key, TObject obj)
        {
            await Task.Factory.StartNew(() => this.Save(key, obj));
        }

        #endregion

        #region Baseline

        public abstract void Delete(TKey key);

        public abstract TObject Create();

        public abstract TObject Load(TKey key);

        /// <summary>
        /// Persists the object using the specified key.
        /// </summary>
        /// <param name="key">
        /// Key to be used when persisting the object.
        /// </param>
        /// <param name="obj">
        /// Object to be persisted.
        /// </param>
        /// <exception cref="CannotSaveObjectException">
        /// The object cannot be saved.
        /// </exception>
        public abstract void Save(TKey key, TObject obj);

        #endregion

        #region Boxed

        void IObjectDeleter.Delete(object key)
        {
            if (!(key is TKey))
                throw new ArgumentException("key must be of type TKey.", "key");

            this.Delete((TKey)key);
        }

        object IObjectFactory.Create()
        {
            return this.Create();
        }

        object IObjectLoader.Load(object key)
        {
            if (!(key is TKey))
                throw new ArgumentException("key must be of type TKey.", "key");

            return this.Load((TKey)key);
        }

        void IObjectSaver.Save(object key, object obj)
        {
            if (!(key is TKey))
                throw new ArgumentException("key must be of type TKey.", "key");

            if (!(obj is TObject))
                throw new ArgumentException("obj must be of type TObject.", "obj");

            this.Save((TKey)key, (TObject)obj);
        }

        #endregion

        #region Implicit IObjectAccessor

        Type IObjectAccessor.ObjectType
        {
            get { return typeof(TObject); }
        }

        Type IObjectAccessor.KeyType
        {
            get { return typeof(TKey); }
        }

        #endregion
    }
}
