import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Paginacao from "../../templates/Paginacao";
// mui table
import {
  Paper,
  styled,
  Table,
  TableBody,
  TableCell,
  tableCellClasses,
  TableContainer,
  TableFooter,
  TableHead,
  TablePagination,
  TableRow,
} from "@mui/material";

// import TablePaginationActions from '@mui/material/TablePagination/TablePaginationActions';
import { getAllMovimentos } from "../../redux/actions/movimentoActions";
import MessageError from "../../templates/Error/MessageError";
import LoadingMessage from "../../templates/Loading/LoadingMessage";

// estilos da tabela
const StyledTableCell = styled(TableCell)(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: theme.palette.common.black,
    color: theme.palette.common.white,
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
  "&:nth-of-type(odd)": {
    backgroundColor: theme.palette.action.hover,
  },
  // hide last border
  "&:last-child td, &:last-child th": {
    border: 0,
  },
}));

const GetMovimentosList = () => {
  const dispatch = useDispatch();

  const getAllMovimentoStore = useSelector(
    (state) => state.getAllMovimentoStore
  );
  const { movimentos, loading, error } = getAllMovimentoStore;

  // paginacao
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);

  // Avoid a layout jump when reaching the last page with empty rows.
  const emptyRows =
    page > 0 ? Math.max(0, (1 + page) * rowsPerPage - movimentos.length) : 0;

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  useEffect(() => {
    dispatch(getAllMovimentos);
  }, [dispatch]);
  return (
    <>
      {error ? (
        <div style={{ maxWidth: "24rem", padding: "2rem" }}>
          <MessageError variant="danger">{error}</MessageError>
        </div>
      ) : loading ? (
        <div style={{ maxWidth: "24rem", marginLeft: "50%" }}>
          <LoadingMessage></LoadingMessage>
        </div>
      ) : (
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 500 }} aria-label="custom pagination table">
            <TableHead>
              <StyledTableRow>
                <StyledTableCell>#</StyledTableCell>
                <StyledTableCell>CATEGORIA</StyledTableCell>
                <StyledTableCell>DESCRIÇÃO</StyledTableCell>
                <StyledTableCell>VALOR</StyledTableCell>
                <StyledTableCell>SUBTOTAL</StyledTableCell>
                <StyledTableCell>DATA</StyledTableCell>
              </StyledTableRow>
            </TableHead>
            <TableBody
              sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
            >
              {(rowsPerPage > 0
                ? movimentos.slice(
                    page * rowsPerPage,
                    page * rowsPerPage + rowsPerPage
                  )
                : movimentos
              ).map((movimento) => {
                const formattedValor = new Intl.NumberFormat("pt-BR", {
                  style: "currency",
                  currency: "AOA", // Se essa é a sua moeda, mantenha como está
                }).format(movimento.valor);

                const formattedCaixa = new Intl.NumberFormat("pt-BR", {
                  style: "currency",
                  currency: "AOA", // Se essa é a sua moeda, mantenha como está
                }).format(movimento.caixa);
                return (
                  <StyledTableRow key={movimento.id}>
                    <StyledTableCell>{movimento.id}</StyledTableCell>
                    <StyledTableCell>{movimento.categoria}</StyledTableCell>
                    <StyledTableCell>{movimento.descricao}</StyledTableCell>
                    <StyledTableCell>{formattedValor}</StyledTableCell>
                    <StyledTableCell>{formattedCaixa}</StyledTableCell>
                    <StyledTableCell>{movimento.dataRegistro}</StyledTableCell>
                  </StyledTableRow>
                );
              })}
              {emptyRows > 0 && (
                <StyledTableRow style={{ height: 53 * emptyRows }}>
                  <StyledTableCell colSpan={6} />
                </StyledTableRow>
              )}
            </TableBody>
            <TableFooter>
              <TableRow>
                <TablePagination
                  colSpan={4}
                  labelDisplayedRows={({ from, to, count }) =>
                    `${from}-${to} de ${count}`
                  }
                  count={movimentos.length}
                  rowsPerPage={rowsPerPage}
                  page={page}
                  SelectProps={{
                    inputProps: {
                      "aria-label": "rows",
                    },
                    native: true,
                  }}
                  rowsPerPageOptions={[2, 5, 10, { label: "tudo", value: -1 }]}
                  labelRowsPerPage={"Linhas por pagina"}
                  onPageChange={handleChangePage}
                  onRowsPerPageChange={handleChangeRowsPerPage}
                  ActionsComponent={Paginacao}
                />
              </TableRow>
            </TableFooter>
          </Table>
        </TableContainer>
      )}
    </>
  );
};
export default GetMovimentosList;
