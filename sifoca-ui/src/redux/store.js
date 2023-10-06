import { configureStore } from "@reduxjs/toolkit";
import { applyMiddleware, combineReducers, compose } from "redux";
import thunk from "redux-thunk";

import * as entrada from "./reducers/entradaReducer.jsx";
import * as saida from "./reducers/saidaReducer.jsx";
import * as movimento from "./reducers/movimentoReducer.jsx";

const reducer = combineReducers({
  // movimentos store
  getAllMovimentoStore: movimento.getAllMovimentosReducer,
  // insertCountryStore: country.insertCountryReducer,
  // deleteCountryStore: country.deleteCountryReducer,
  // updateCountryStore: country.updateCountryReducer,

  // entradas store
  getAllEntradaStore: entrada.getAllEntradasReducer,
  // insertCountryStore: country.insertCountryReducer,
  // deleteCountryStore: country.deleteCountryReducer,
  // updateCountryStore: country.updateCountryReducer,

  // saidas store
  getAllSaidaStore: saida.getAllSaidasReducer,
  // insertCountryStore: country.insertCountryReducer,
  // deleteCountryStore: country.deleteCountryReducer,
  // updateCountryStore: country.updateCountryReducer,
});

const compositor = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = configureStore({ reducer }, compositor(applyMiddleware(thunk)));

export default store;
