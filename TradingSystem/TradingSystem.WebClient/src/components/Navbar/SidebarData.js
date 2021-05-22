import React from 'react';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as HiIcons from 'react-icons/hi';

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
        title: 'Store Management',
        path: '/myStores',
        icon: <FaIcons.FaStore />,
        cName: 'nav-text'
    },
    {
        title: 'Users Management',
        path: '/team',
        icon: <IoIcons.IoMdPeople />,
        cName: 'nav-text'
    },
    {
        title: 'History',
        path: '/messages',
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
