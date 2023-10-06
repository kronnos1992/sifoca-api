import { ExpandLess, ExpandMore } from "@mui/icons-material";
import React, { useState } from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";

const SidebarLink = styled(Link)`
  display: flex;
  justify-content: flex-start;
  padding-bottom: 10px;
  color: #3a3f41;
  padding: 1.3rem;
  list-style: none;
  max-height: 10%;
  text-decoration: none;
  font-size: 20px;
  transition: all 0.3s ease-in-out;
  &:hover {
    background: #000000;
    border-left: 4px solid #cfa414;
    color: #b2c8d2;
    transition: 0.3s cubic-bezier(0.6, -0.28, 0.735, 0.045);
    cursor: pointer;
  }
`;

const DropdownLink = styled(Link)`
  height: 40px;
  padding-left: 3rem;
  display: flex;
  align-items: center;
  text-decoration: none;
  color: #000000;
  font-size: 14px;
  &:hover {
    background: #000000;
    border-left: 4px solid #cfa414;
    transition: 0.3s cubic-bezier(0.6, -0.28, 0.735, 0.045);
    cursor: pointer;
    color: #f0f0f0;
  }
`;
const SidebarLabel = styled.span`
  margin-left: 12px;
`;

const SubMenu = ({ item }) => {
  // buscar subNav no sidePaths com useState
  const [subnav, setSubNav] = useState(false);
  const openSubnav = () => setSubNav(!subnav);

  return (
    <>
      <SidebarLink to={item.path} onClick={item.subNav && openSubnav}>
        {item.icon}
        <SidebarLabel>{item.title}</SidebarLabel>
        <span>
          {item.subNav && subnav ? (
            <ExpandMore />
          ) : item.subNav ? (
            <ExpandLess />
          ) : null}
        </span>
      </SidebarLink>
      {subnav &&
        item.subNav.map((item, index) => {
          return (
            <DropdownLink to={item.path} key={index}>
              {item.icon}
              <SidebarLabel>{item.title}</SidebarLabel>
            </DropdownLink>
          );
        })}
    </>
  );
};
export default SubMenu;
