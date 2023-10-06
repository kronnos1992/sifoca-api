import React, { useEffect } from "react";
import { useDispatch } from "react-redux";

import { getAllMovimentos } from "../../redux/actions/movimentoActions";
import GetMovimentosList from "./GetMovimentoList";

const Movimento = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(getAllMovimentos);
  }, [dispatch]);

  return (
    <div style={{ maxWidth: "70%", margin: "auto" }}>
      <h4 className="text-center">RELATÓRIO DE MOVIMENTOS</h4>
      <GetMovimentosList />
    </div>
  );
};

export default Movimento;
