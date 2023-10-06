import {
  AccountBalance,
  InsertChart,
  InsightsTwoTone,
  Outbound,
} from "@mui/icons-material";

const SidePath = [
  {
    title: "MOVIMENTOS",
    icon: <InsertChart />,
    subNav: [
      {
        title: "Geral",
        icon: <AccountBalance />,
        path: "/movimentos",
      },
      {
        title: "Entradas",
        icon: <InsightsTwoTone />,
        path: "/entradas",
      },
      {
        title: "Saídas",
        icon: <Outbound />,
        path: "/saidas",
      },
    ],
  },
];
export default SidePath;
