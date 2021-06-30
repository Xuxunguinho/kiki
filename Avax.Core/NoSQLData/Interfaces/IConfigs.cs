using System;
using System.Collections.Generic;
using Avax.Core.NoSQLData.Objects;

namespace Avax.Core.NoSQLData.Interfaces
{
    public  interface IConfigs
    {
        ConfigAvaliador Get(int curso , int classe);
        List<ConfigAvaliador> Items { get; }
        ConfigAvaliador this[int index] { get; set; }
        List<ConfigAvaliador> GetData();

        /// <summary>
        /// Sobscrever o FileTable Inteiro e salvar apenas este item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Override(ConfigAvaliador item);

        /// <summary>
        /// Sobscrever o FileTable Inteiro e salvar apenas estes items
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool Override(List<ConfigAvaliador> items);

        /// <summary>
        /// Insere novo item ao FileTable
        /// </summary>
        /// <param name="item"> Item a ser Inserido</param>
        bool Insert(ConfigAvaliador item);

        /// <summary>
        /// Insere ou Atualiza um item na Tabela
        /// </summary>
        /// <param name="item"> Item a ser Inserido </param>
        /// <param name="setter"></param>
        /// <param name="equater"></param>
        bool Insert(ConfigAvaliador item, Action<ConfigAvaliador, ConfigAvaliador> setter, Func<ConfigAvaliador, ConfigAvaliador, bool> equater);

        /// <summary>
        /// Insere uma Lista de  novos items ao FileTable
        /// </summary>
        /// <param name="items">Items a serem Inseridos</param>
        bool Insert(List<ConfigAvaliador> items);

        /// <summary>
        /// Insere  ou atualiza uma Lista os dados na tablela
        /// </summary>
        /// <param name="items">Items a serem Inseridos</param>
        /// <param name="setter"> 'TIn,TOut' metodo para setar os dados pretendidos</param>
        /// <param name="equater">'TIn,TOut' Função condicional => representa a opcao Where</param>
        /// <returns></returns>
        bool Insert(List<ConfigAvaliador> items, Action<ConfigAvaliador, ConfigAvaliador> setter, Func<ConfigAvaliador, ConfigAvaliador, bool> equater);

        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable
        /// </summary>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        bool Update(Action<ConfigAvaliador> setter, Func<ConfigAvaliador, bool> equater);

        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable apartir de uma lista referente
        /// </summary>
        /// <param name="source"></param>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        bool Update(List<ConfigAvaliador> source, Action<ConfigAvaliador, ConfigAvaliador> setter, Func<ConfigAvaliador, ConfigAvaliador, bool> equater);

        /// <summary>
        /// Elimina do FileTable todos items representados pela função condicional
        /// </summary>
        /// <param name="equater">Função condicional</param>
        bool Delete(Func<ConfigAvaliador, bool> equater);
    }
}