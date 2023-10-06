import React, { useEffect } from "react";
import { useDispatch } from "react-redux";

import { getAllEntradas } from "../../redux/actions/entradaActions";
import GetEntradasList from "./GetEntradaList";

const Entrada = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(getAllEntradas);
  }, [dispatch]);

  return (
    <div style={{ maxWidth: "70%", margin: "auto" }}>
      <h4 className="text-center">RELATÓRIO DE VENDAS</h4>
      <GetEntradasList />
    </div>
  );
};

export default Entrada;
