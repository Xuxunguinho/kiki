using System;
using System.Collections.Generic;
using Avax.Core.NoSQLData_Examples.Objects;

namespace Avax.Core.NoSQLData_Examples.Interfaces
{
    public  interface IConfigs
    {
        Configuration Get(int curso , int classe);
        List<Configuration> Items { get; }
        Configuration this[int index] { get; set; }
        List<Configuration> GetData();

        /// <summary>
        /// Sobscrever o FileTable Inteiro e salvar apenas este item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Override(Configuration item);

        /// <summary>
        /// Sobscrever o FileTable Inteiro e salvar apenas estes items
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool Override(List<Configuration> items);

        /// <summary>
        /// Insere novo item ao FileTable
        /// </summary>
        /// <param name="item"> Item a ser Inserido</param>
        bool Insert(Configuration item);

        /// <summary>
        /// Insere ou Atualiza um item na Tabela
        /// </summary>
        /// <param name="item"> Item a ser Inserido </param>
        /// <param name="setter"></param>
        /// <param name="equater"></param>
        bool Insert(Configuration item, Action<Configuration, Configuration> setter, Func<Configuration, Configuration, bool> equater);

        /// <summary>
        /// Insere uma Lista de  novos items ao FileTable
        /// </summary>
        /// <param name="items">Items a serem Inseridos</param>
        bool Insert(List<Configuration> items);

        /// <summary>
        /// Insere  ou atualiza uma Lista os dados na tablela
        /// </summary>
        /// <param name="items">Items a serem Inseridos</param>
        /// <param name="setter"> 'TIn,TOut' metodo para setar os dados pretendidos</param>
        /// <param name="equater">'TIn,TOut' Função condicional => representa a opcao Where</param>
        /// <returns></returns>
        bool Insert(List<Configuration> items, Action<Configuration, Configuration> setter, Func<Configuration, Configuration, bool> equater);

        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable
        /// </summary>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        bool Update(Action<Configuration> setter, Func<Configuration, bool> equater);

        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable apartir de uma lista referente
        /// </summary>
        /// <param name="source"></param>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        bool Update(List<Configuration> source, Action<Configuration, Configuration> setter, Func<Configuration, Configuration, bool> equater);

        /// <summary>
        /// Elimina do FileTable todos items representados pela função condicional
        /// </summary>
        /// <param name="equater">Função condicional</param>
        bool Delete(Func<Configuration, bool> equater);
    }
}