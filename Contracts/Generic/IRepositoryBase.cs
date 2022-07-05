using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Contracts.Generic
{
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Realiza una busqueda por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T find(object id);

        /// <summary>
        /// Busca todos los registros del objeto, NO incluye hijos
        /// </summary>
        /// <returns></returns>
        IQueryable<T> findAll();

        /// <summary>
        /// Busca todos los registros del objeto, incluye hijos
        /// </summary>
        /// <param name="toInclude">string</param>
        /// <returns></returns>
        IQueryable<T> findAll(string toInclude);

        /// <summary>
        /// Busca segun una condicion entregada, NO incluye hijos
        /// </summary>
        /// <param name="expression">Expresion</param>
        /// <returns></returns>
        IQueryable<T> findByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Busca segun una condicion entregada, incluye hijos
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="toInclude"></param>
        /// <returns></returns>
        IQueryable<T> findByCondition(Expression<Func<T, bool>> expression,string toInclude);

        /// <summary>
        /// Crea un registro en la bases de datos al recibir un objeto de la clase seleccionada
        /// </summary>
        /// <param name="entity"></param>
        void create(T entity);

        /// <summary>
        /// Modifica un registro existente
        /// </summary>
        /// <param name="entity"></param>
        void update(T entity);

        /// <summary>
        /// Elimina un registro existente
        /// </summary>
        /// <param name="entity"></param>
        void delete(T entity);
    }
}
