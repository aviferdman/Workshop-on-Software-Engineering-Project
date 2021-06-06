import React from 'react';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as HiIcons from 'react-icons/hi';
import {UserRole} from "../../globalContext";

export const SidebarData = [
    {
        title: 'Home',
        path: '/home',
        icon: <AiIcons.AiFillHome />,
        cName: 'nav-text'
    },
    {
        title: 'Shopping Cart',
        path: '/ShoppingCart',
        icon: <HiIcons.HiShoppingCart />,
        cName: 'nav-text'
    },

    {
        title: 'Search Store',
        path: '/searchStore',
        icon: <FaIcons.FaStoreAlt />,
        cName: 'nav-text'
    },

    {
        title: 'Store Management',
        path: '/myStores',
        icon: <FaIcons.FaStore />,
        cName: 'nav-text'
    },
    {
        title: 'Admin Actions',
        path: '/adminActions',
        icon: <IoIcons.IoMdPeople />,
        cName: 'nav-text',
        permissions: [UserRole.admin]
    },
    {
        title: 'My Bids',
        path: '/userBids',
        icon: <AiIcons.AiOutlineHistory />,
        cName: 'nav-text'
    },
    {
        title: 'History',
        path: '/userHistory',
        icon: <AiIcons.AiOutlineHistory />,
        cName: 'nav-text'
    },
    {
        title: 'Support',
        path: '/support',
        icon: <IoIcons.IoMdHelpCircle />,
        cName: 'nav-text'
    }
];
