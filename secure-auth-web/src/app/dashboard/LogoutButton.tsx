'use client';

import { api } from '@/lib/api';
import { useRouter } from 'next/navigation';

export default function LogoutButton() {
	const router = useRouter();

	const handleLogout = async () => {
		try {
			await api.post('/auth/logout');
			router.push('/login');
			router.refresh();
		} catch (error) {
			console.error('Logout failed', error);
		}
	};

	return (
		<button
			onClick={handleLogout}
			className='bg-red-600 text-white px-6 py-2 rounded-md hover:bg-red-700 transition'
		>
			Sign Out
		</button>
	);
}
